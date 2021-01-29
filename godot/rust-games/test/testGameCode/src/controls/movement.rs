use gdnative::api::KinematicBody2D;
use gdnative::prelude::*;
use std::ops::Add;

pub struct Drag<T> where T: GodotObject {
    value: f32,
    direction: Box<dyn Fn(&T, &Vector2) -> Vector2>,
    // (target, new_movement) -> drag_direction
    source: i64,
}

impl <T> Drag<T> where T: GodotObject {
    pub fn new(value: f32, direction: impl Fn(&T, &Vector2) -> Vector2 + 'static, source_id: i64) -> Self {
        Drag {
            value,
            direction: Box::new(direction),
            source: source_id,
        }
    }
}

pub struct BasicMovementStats<T> where T: GodotObject {
    pub(crate) enabled: bool,
    pub(crate) drag: Vec<Drag<T>>,
    pub(crate) screen_size: Vector2,
}

impl <T> BasicMovementStats<T> where T: GodotObject {
    pub fn new() -> Self {
        BasicMovementStats {
            enabled: true,
            drag: Vec::new(),
            screen_size: Vector2::zero(),
        }
    }
}

pub trait MovementController {
    // get and set stats
    fn get_basic_movement_stats(&self) -> &BasicMovementStats<KinematicBody2D>;
    fn get_basic_movement_stats_mut(&mut self) -> &mut BasicMovementStats<KinematicBody2D>;
    fn set_movement_enabled(&mut self, enabled: bool) {
        self.get_basic_movement_stats_mut().enabled = enabled;
    }

    fn add_movement_drag(&mut self, drag: Drag<KinematicBody2D>) {
        self.get_basic_movement_stats_mut().drag.push(drag);
    }
    fn remove_movement_drag(&mut self, source_id: i64) {
        let index = self.get_basic_movement_stats_mut().drag.iter().position(|drag| drag.source == source_id).unwrap();
        self.get_basic_movement_stats_mut().drag.remove(index);
    }

    fn movement_ready(&mut self, owner: &KinematicBody2D) {
        // setup remaining basic stats
        let viewport = owner.get_viewport_rect();
        self.get_basic_movement_stats_mut().screen_size = viewport.size.to_vector();

        // setup controller itself
        self.movement_ready_internal(owner);
    }
    fn movement_ready_internal(&mut self, owner: &KinematicBody2D);

    // process updates
    fn get_movement(&self, _owner: &KinematicBody2D) -> Vector2;
    fn movement_process(&mut self, owner: &KinematicBody2D, _delta: f32) {
        let mut movement = Vector2::zero();
        let basic_stats = self.get_basic_movement_stats();
        let current_pos: Vector2 = owner.global_position();
        // custom movement
        if basic_stats.enabled {
            movement = movement.add(self.get_movement(owner));
            owner.look_at(current_pos.add(movement));
        }

        // drag
        for drag in basic_stats.drag.iter() {
            let drag_direction = (drag.direction)(owner, &movement);
            //godot_print!("drag: {}, {}", drag_direction.x, drag_direction.y);
            movement = movement.add(drag_direction * drag.value);
        }
        // apply movement
        owner.move_and_slide(movement, Vector2::zero(), false, 24, 1.5, false);
    }
}