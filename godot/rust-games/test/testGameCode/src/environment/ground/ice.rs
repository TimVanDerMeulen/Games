use gdnative::api::*;
use gdnative::prelude::*;
use crate::casting::{ScriptCaster, NodeCaster};
use crate::controls::movement::Drag;

#[derive(NativeClass)]
#[inherit(Area2D)]
pub struct Ice {
    drag: f32,
}

#[methods]
impl Ice {
    fn new(_owner: &Area2D) -> Self {
        Ice {
            drag: 200.0,
        }
    }

    #[export]
    fn body_entered(&mut self, owner: &Area2D, entered: Variant) {
        if let Some(kin_body_ref) = entered.try_as_kinematic_body2d_ref() {
            let direction: Vector2 = Rotation2D::new(Angle::degrees(unsafe { kin_body_ref.assume_safe().rotation_degrees() } as f32)).transform_vector(Vector2::new(1.0,0.0)).normalize();
            let strength = self.drag;
            kin_body_ref.try_as_movement_controller(move |movement_controller| movement_controller.add_movement_drag(Drag::new(strength, move | _target, _new_direction | direction, owner.get_instance_id())));
            self.set_movement_enabled(false, kin_body_ref);
        }
    }
    #[export]
    fn body_exited(&mut self, owner: &Area2D, exited: Variant) {
        if let Some(kin_body_ref) = exited.try_as_kinematic_body2d_ref() {
            kin_body_ref.try_as_movement_controller(|movement_controller| movement_controller.remove_movement_drag(owner.get_instance_id()));
            self.set_movement_enabled(true, kin_body_ref);
        }
    }

    fn set_movement_enabled(&mut self, enabled: bool, kin_body_ref: Ref<KinematicBody2D, Shared>) {
        kin_body_ref.try_as_movement_controller(|movement_controller| {
            movement_controller.set_movement_enabled(enabled);
        });
    }
}