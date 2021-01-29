use std::any::Any;

use crate::player::Player;
use crate::interaction::{Interactable, InventoryManager, Collectable, CollectableType};
use gdnative::prelude::*;

impl Interactable for Player {
    fn interact(&mut self, other: &dyn Any) {
        let mut interaction_performed: bool = false;
        //if (self == other) {
        //    godot_print!("use current item on self");
        //    interaction_performed = true;
        //}
        if let Some(_casted_other) = other.downcast_ref::<Player>() {
            godot_print!("interacting with other player");
            interaction_performed = true;
        }
        if !interaction_performed {
            godot_print!("interaction default");
        }
    }
}

impl InventoryManager for Player {
    fn collect(&mut self, other: &dyn Collectable) {
        godot_print!("collect smth");

        self.inventory.add(other);
        match other.get_collectable_type() {
            CollectableType::FOOD => godot_print!("collected: food"),
        }
    }
}