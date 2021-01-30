pub mod walker;

use gdnative::prelude::*;
use crate::controls::movement::{BasicMovementStats, MovementController};
use crate::mobs::enemies::walker::Walker;

pub trait Enemy {

}

pub struct EnemyMovementStats {
    basic: BasicMovementStats<KinematicBody2D>,
    speed: f32,
}

impl EnemyMovementStats {
    pub(crate) fn new() -> Self {
        EnemyMovementStats {
            basic: BasicMovementStats::new(),
            speed: 160.0,
        }
    }
}

pub trait EnemyMovementController: MovementController {
    fn get_basic_enemy_movement_stats(&self) -> &BasicMovementStats<KinematicBody2D>;
    fn get_basic_enemy_movement_stats_mut(&mut self) -> &mut BasicMovementStats<KinematicBody2D>;
}

impl <T> MovementController for T where T: Enemy + EnemyMovementController {

    fn get_basic_movement_stats(&self) -> &BasicMovementStats<KinematicBody2D> {
        self.get_basic_enemy_movement_stats()
    }
    fn get_basic_movement_stats_mut(&mut self) -> &mut BasicMovementStats<KinematicBody2D> {
        self.get_basic_enemy_movement_stats_mut()
    }

    fn movement_ready_internal(&mut self, owner: &KinematicBody2D) {
        gdnative::prelude::godot_print!("enemy ready");
    }

    fn get_movement(&self, _owner: &KinematicBody2D) -> Vector2 {
        //let player = Input::godot_singleton();
        let mut velocity = Vector2::new(0.0, 0.0);
        return velocity;
    }
}