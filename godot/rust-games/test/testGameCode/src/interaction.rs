use std::any::Any;
use gdnative::prelude::*;

pub trait Interactable {
    fn interact(&mut self, other: &dyn Any);
}

pub enum CollectableType {
    FOOD
}

#[derive(Clone)]
pub struct CollectableState {
    pub collected: bool
}

impl CollectableState {
    pub fn new() -> Self {
        CollectableState {
            collected: false,
        }
    }

    pub fn mark_as_collected(&mut self){
        self.collected = true;
    }
}

pub trait Collectable {
    fn get_collectable_type(&self) -> CollectableType;
    fn get_collectable_state(&self) -> CollectableState;
    fn collected_by(&mut self, _collector: &dyn InventoryManager);
}

pub struct Inventory {
    collectables: Vec<CollectableState>,
}

impl Inventory {

    pub fn new() -> Self {
        Inventory {
            collectables: Vec::new(),
        }
    }

    pub fn add(&mut self, collectable: &dyn Collectable) {
        godot_print!("inventory: collectable added!");
        self.collectables.push(collectable.get_collectable_state());
    }

    //pub fn get_collectables(&self) -> Vec<CollectableState> {
    //    return self.collectables.to_vec();
    //}
}

pub trait InventoryManager {
    fn collect(&mut self, other: &dyn Collectable);
}