use gdnative::api::Input;
use gdnative::prelude::*;
use crate::player::Player;
use crate::controls::movement::{BasicMovementStats, MovementController};

pub struct PlayerMovementStats {
    basic: BasicMovementStats<KinematicBody2D>,
    speed: f32,
}

impl PlayerMovementStats {
    pub(crate) fn new() -> Self {
        PlayerMovementStats {
            basic: BasicMovementStats::new(),
            speed: 250.0,
        }
    }
}

impl MovementController for Player {
    fn get_basic_movement_stats(&self) -> &BasicMovementStats<KinematicBody2D> {
        &self.movement.basic
    }
    fn get_basic_movement_stats_mut(&mut self) -> &mut BasicMovementStats<KinematicBody2D> {
        &mut self.movement.basic
    }

    fn movement_ready_internal(&mut self, owner: &KinematicBody2D) {
        gdnative::prelude::godot_print!("controllable ready: {}", owner.name());
    }

    fn get_movement(&self, _owner: &KinematicBody2D) -> Vector2 {
        let input = Input::godot_singleton();
        let mut velocity = Vector2::new(0.0, 0.0);

        if Input::is_action_pressed(&input, "player_left") {
            velocity.x -= 1.0;
        }
        if Input::is_action_pressed(&input, "player_right") {
            velocity.x += 1.0;
        }
        if Input::is_action_pressed(&input, "player_up") {
            velocity.y -= 1.0;
        }
        if Input::is_action_pressed(&input, "player_down") {
            velocity.y += 1.0;
        }

        if velocity.length() > 0.0 {
            velocity = velocity.normalize() * self.movement.speed;
        }

        return velocity;
    }
}