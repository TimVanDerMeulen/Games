use gdnative::api::*;
use gdnative::prelude::*;
use crate::casting::{ScriptCaster, NodeCaster};
use crate::controls::movement::Drag;

#[derive(NativeClass)]
#[inherit(Area2D)]
pub struct Magnet {
    drag: f32,
    center: Vector2,
}

#[methods]
impl Magnet {
    fn new(_owner: &Area2D) -> Self {
        Magnet {
            drag: 0.4,
            center: Vector2::zero(),
        }
    }

    #[export]
    fn _ready(&mut self, owner: &Area2D) {
        self.center = owner.global_position();
    }

    #[export]
    fn body_entered(&mut self, owner: &Area2D, entered: Variant) {
        if let Some(kin_body_ref) = entered.try_as_kinematic_body2d_ref() {
            let drag_point: Vector2 = self.center;
            let strength = self.drag;
            kin_body_ref.try_as_movement_controller(move |movement_controller| movement_controller.add_movement_drag(Drag::new(strength, move | target: &KinematicBody2D, _new_direction | drag_point - target.global_position(), owner.get_instance_id())));
        }
    }
    #[export]
    fn body_exited(&mut self, owner: &Area2D, exited: Variant) {
        if let Some(kin_body_ref) = exited.try_as_kinematic_body2d_ref() {
            kin_body_ref.try_as_movement_controller(|movement_controller| movement_controller.remove_movement_drag(owner.get_instance_id()));
        }
    }

}