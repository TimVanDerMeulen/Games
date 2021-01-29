use gdnative::api::KinematicBody2D;
use gdnative::prelude::*;

use crate::interaction::Inventory;
use crate::player::controls::movement::PlayerMovementStats;
use crate::controls::movement::MovementController;

mod controls;

#[derive(NativeClass)]
#[inherit(KinematicBody2D)]
pub struct Player {
    movement: PlayerMovementStats,
    inventory: Inventory,
}

#[methods]
impl Player {
    fn new(_owner: &KinematicBody2D) -> Self {
        Player {
            movement: PlayerMovementStats::new(),
            inventory: Inventory::new(),
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