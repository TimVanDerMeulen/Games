use gdnative::api::Area2D;
use gdnative::prelude::*;
use crate::casting::{ScriptCaster, NodeCaster};
use crate::controls::movement::Drag;
use std::ops::Neg;

#[derive(NativeClass)]
#[inherit(Area2D)]
pub struct Mud {
    drag: f32,
}

#[methods]
impl Mud {
    fn new(_owner: &Area2D) -> Self {
        Mud {
            drag: 0.6,
        }
    }

    #[export]
    fn body_entered(&mut self, owner: &Area2D, entered: Variant) {
        if let Some(kin_body_ref) = entered.try_as_kinematic_body2d_ref() {
            let strength = self.drag;
            kin_body_ref.try_as_movement_controller(move |movement_controller| movement_controller.add_movement_drag(Drag::new(strength, move |_target, new_direction| new_direction.neg(), owner.get_instance_id())));
        }
    }
    #[export]
    fn body_exited(&mut self, owner: &Area2D, exited: Variant) {
        if let Some(kin_body_ref) = exited.try_as_kinematic_body2d_ref() {
            kin_body_ref.try_as_movement_controller(|movement_controller| movement_controller.remove_movement_drag(owner.get_instance_id()));
        }
    }

}