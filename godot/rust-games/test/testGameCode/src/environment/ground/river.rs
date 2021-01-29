use gdnative::api::Area2D;
use gdnative::prelude::*;
use crate::casting::{ScriptCaster, NodeCaster};
use crate::controls::movement::Drag;

#[derive(NativeClass)]
#[inherit(Area2D)]
pub struct River {
    #[property(default = 300.0)]
    drag: f32,
    #[property]
    direction: Vector2,
}

#[methods]
impl River {
    fn new(_owner: &Area2D) -> Self {
        River {
            drag: 300.0,
            direction: Vector2::zero(),
        }
    }
    #[export]
    fn _ready(&mut self, _owner: &Area2D) {
        self.direction = self.direction.normalize();
    }

    #[export]
    fn body_entered(&mut self, owner: &Area2D, entered: Variant) {
        if let Some(kin_body_ref) = entered.try_as_kinematic_body2d_ref() {
            let direction = self.direction;
            let strength = self.drag;
            kin_body_ref.try_as_movement_controller(move |movement_controller| movement_controller.add_movement_drag(Drag::new(strength, move |_target, _new_direction| direction, owner.get_instance_id())));
        }
    }
    #[export]
    fn body_exited(&mut self, owner: &Area2D, exited: Variant) {
        if let Some(kin_body_ref) = exited.try_as_kinematic_body2d_ref() {
            kin_body_ref.try_as_movement_controller(|movement_controller| movement_controller.remove_movement_drag(owner.get_instance_id()));
        }
    }
}