use gdnative::prelude::*;
use gdnative::api::*;
use crate::interaction::{InventoryManager, Interactable};
use crate::player::Player;
use crate::controls::movement::MovementController;
use crate::controls::actions::ActionController;

pub trait NodeCaster {
    fn try_as_area2d_ref(&self) -> Option<Ref<Area2D, Shared>>;
    fn try_as_kinematic_body2d_ref(&self) -> Option<Ref<KinematicBody2D, Shared>>;
}

impl NodeCaster for Variant {
    fn try_as_area2d_ref(&self) -> Option<Ref<Area2D, Shared>> {
        return self.try_to_object::<Area2D>();
    }
    fn try_as_kinematic_body2d_ref(&self) -> Option<Ref<KinematicBody2D, Shared>> {
        return self.try_to_object::<KinematicBody2D>();
    }
}

pub trait ScriptCaster {
    fn try_as_inventory<F>(&self, do_on_match: F) where F: FnOnce(&mut dyn InventoryManager) -> ();
    fn try_as_interactable<F>(&self, do_on_match: F) where F: FnOnce(&mut dyn Interactable) -> ();
    fn try_as_movement_controller<F>(&self, do_on_match: F) where F: FnOnce(&mut dyn MovementController) -> ();
    fn try_as_action_controller<F>(&self, do_on_match: F) where F: FnOnce(&mut dyn ActionController) -> ();
}

impl ScriptCaster for Ref<KinematicBody2D, Shared> {
    fn try_as_inventory<F>(&self, do_on_match: F) where F: FnOnce(&mut dyn InventoryManager) -> () {
        if let Some(player_ref_instance) = unsafe { self.assume_safe().cast_instance::<Player>() } {
            player_ref_instance.map_mut(|player, _| do_on_match(player)).expect("Failed to extract player from ref instance");
            return;
        }
    }

    fn try_as_interactable<F>(&self, do_on_match: F) where F: FnOnce(&mut dyn Interactable) -> () {
        if let Some(player_ref_instance) = unsafe { self.assume_safe().cast_instance::<Player>() } {
            player_ref_instance.map_mut(|player, _| do_on_match(player)).expect("Failed to extract player from ref instance");
            return;
        }
    }

    fn try_as_movement_controller<F>(&self, do_on_match: F) where F: FnOnce(&mut dyn MovementController) -> () {
        if let Some(player_ref_instance) = unsafe { self.assume_safe().cast_instance::<Player>() } {
            player_ref_instance.map_mut(|player, _| do_on_match(player)).expect("Failed to extract player from ref instance");
            return;
        }
    }

    fn try_as_action_controller<F>(&self, do_on_match: F) where F: FnOnce(&mut dyn ActionController) -> () {
        if let Some(player_ref_instance) = unsafe { self.assume_safe().cast_instance::<Player>() } {
            player_ref_instance.map_mut(|player, _| do_on_match(player)).expect("Failed to extract player from ref instance");
            return;
        }
    }
}