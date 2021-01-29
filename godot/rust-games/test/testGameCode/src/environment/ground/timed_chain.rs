use std::time::Duration;

use gdnative::api::*;
use gdnative::prelude::*;

use crate::casting::{NodeCaster, ScriptCaster};
use crate::controls::movement::Drag;

#[derive(NativeClass)]
#[inherit(Area2D)]
#[register_with(Self::register_signals)]
pub struct TimedChain {
    center: Vector2,
    radius: f64,
    chain_duration: Duration,
}

#[methods]
impl TimedChain {
    fn new(_owner: &Area2D) -> Self {
        TimedChain {
            center: Vector2::zero(),
            radius: 0.0,
            chain_duration: Duration::from_secs(3),
        }
    }

    fn register_signals(builder: &ClassBuilder<Self>) {
        builder.add_signal(Signal {
            name: "release",
            // Argument list used by the editor for GUI and generation of GDScript handlers. It can be omitted if the signal is only used from code.
            args: &[SignalArgument {
                name: "to_release",
                default: Default::default(),
                export_info: ExportInfo::new(VariantType::VariantArray),
                usage: PropertyUsage::DEFAULT,
            }],
        });
    }

    #[export]
    fn _ready(&mut self, owner_ref: TRef<Area2D>) {
        let owner = owner_ref.as_ref();
        self.center = owner.global_position();
        //self.radius = ((owner.get_child(0) as CollisionShape2D).get_shape() as CircleShape2D).radius();
        if let Some(collision_node_ref) = owner.get_node("CollisionShape2D") {
            if let Some(collision_shape) = unsafe { collision_node_ref.assume_safe().cast::<CollisionShape2D>().as_ref() } {
                if let Some(circle_shape) = collision_shape.shape() {
                    if let Some(circle) = unsafe { circle_shape.assume_safe().cast::<CircleShape2D>().as_ref() } {
                        self.radius = circle.radius();
                    }
                }
            }
        }
    }

    #[export]
    unsafe fn body_entered(&mut self, owner: &Area2D, entered: Variant) {
        if let Some(kin_body_ref) = entered.try_as_kinematic_body2d_ref() {
            let radius = self.radius;
            let drag_point = self.center;
            kin_body_ref.try_as_movement_controller(move |movement_controller| movement_controller.add_movement_drag(Drag::new(1.0, move |target: &KinematicBody2D, _new_direction| {
                let distance_vec = drag_point - target.global_position();
                let distance = distance_vec.length();
                if distance > radius as f32 {
                    return distance_vec.normalize() * (distance - radius as f32) * 10.0;
                }
                return Vector2::zero();
            }, owner.get_instance_id())));

            // release timer
            let timer = Timer::new();
            timer.set_one_shot(true);
            timer.set_autostart(true);
            timer.set_wait_time(self.chain_duration.as_secs() as f64);
            //timer.start(self.chain_duration.as_secs() as f64);


            let args = VariantArray::new();
            args.push(entered);
            //args.push(Variant::from_node_path(&timer.get_path()));
            timer.connect(
                "timeout",
                owner.get_parent().unwrap().assume_safe().as_ref().get_node(owner.get_path()).unwrap(),
                "release",
                args.into_shared(),
                0,
            ).unwrap();

            owner.add_child(timer, false);
        }
    }

    #[export]
    fn release(&self, owner: &Area2D, to_release: Variant/*, timer_id: NodePath*/) {
        //if let Some(timer) = owner.get_node(timer_id) {
        //    owner.remove_child(timer);
        //    //timer.queue_free();
        //}
        let owner_id = owner.get_instance_id();
        if let Some(kin_body_ref) = to_release.try_as_kinematic_body2d_ref() {
            kin_body_ref.try_as_movement_controller(|movement_controller| {
                movement_controller.remove_movement_drag(owner_id);
            });
        }
    }
}