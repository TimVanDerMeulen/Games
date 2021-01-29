use gdnative::prelude::*;

use crate::objects::collectables::apple::Apple;
use crate::player::Player;
use crate::environment::ground::ice::Ice;
use crate::environment::ground::mud::Mud;
use crate::environment::ground::magnet::Magnet;
use crate::environment::ground::river::River;
use crate::environment::ground::timed_chain::TimedChain;

mod player;
mod interaction;
mod objects;
mod casting;
mod environment;
mod controls;
mod globals;

fn init(handle: InitHandle) {
    // ############ player #############
    handle.add_class::<Player>();

    // ############ objects ############
    // collectables
    handle.add_class::<Apple>();

    // ########## environment ##########
    // ground
    handle.add_class::<Ice>();
    handle.add_class::<Mud>();
    handle.add_class::<Magnet>();
    handle.add_class::<River>();
    handle.add_class::<TimedChain>();
}

godot_init!(init);