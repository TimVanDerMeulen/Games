use gdnative::api::Area2D;
use gdnative::prelude::*;

use crate::casting::{NodeCaster, ScriptCaster};
use crate::interaction::{Collectable, CollectableType, InventoryManager, CollectableState};

#[derive(NativeClass)]
#[inherit(Area2D)]
pub struct Apple {
    pub test: &'static str,
    state: CollectableState,
}

#[gdnative::methods]
impl Apple {
    fn new(_owner: &Area2D) -> Self {
        Apple {
            test: "blub",
            state: CollectableState::new(),
        }
    }

    #[export]
    fn _ready(&mut self, _owner: &Area2D) {}

    #[export]
    fn _process(&mut self, owner: &Area2D, _delta: f32) {
        if self.state.collected {
            unsafe {
                owner.assume_unique().queue_free();
            }
        }
    }

    #[export]
    fn area_entered(&mut self, _owner: &Area2D, entered: Variant) {
        if let Some(kin_body_ref) = entered.try_as_kinematic_body2d_ref() {
            kin_body_ref.try_as_inventory(|inventory| {
                inventory.collect(self);
                self.collected_by(inventory);
            })
        }
    }

}

impl Collectable for Apple {
    fn get_collectable_type(&self) -> CollectableType {
        CollectableType::FOOD
    }

    fn get_collectable_state(&self) -> CollectableState {
        self.state.clone()
    }

    fn collected_by(&mut self, _collector: &dyn InventoryManager) {
            self.state.mark_as_collected();
    }
}