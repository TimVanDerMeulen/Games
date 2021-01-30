use gdnative::api::KinematicBody2D;
use gdnative::prelude::*;
use crate::mobs::enemies::{EnemyMovementStats, Enemy, EnemyMovementController};
use crate::controls::movement::{MovementController, BasicMovementStats};

#[derive(NativeClass)]
#[inherit(KinematicBody2D)]
pub struct Walker {
    movement: EnemyMovementStats,
}

#[methods]
impl Walker {
    fn new(_owner: &KinematicBody2D) -> Self {
        Walker {
            movement: EnemyMovementStats::new(),
        }
    }

    #[export]
    fn _ready(&mut self, owner: &KinematicBody2D) {
        self.movement_ready(owner);
    }

    #[export]
    fn _process(&mut self, owner: &KinematicBody2D, delta: f32) {
        self.movement_process(owner, delta);
    }

}

impl Enemy for Walker {

}

impl EnemyMovementController for Walker {
    fn get_basic_enemy_movement_stats(&self) -> &BasicMovementStats<KinematicBody2D> {
        &self.movement.basic
    }
    fn get_basic_enemy_movement_stats_mut(&mut self) -> &mut BasicMovementStats<KinematicBody2D> {
        &mut self.movement.basic
    }
}