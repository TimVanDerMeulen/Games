struct GlobalControl {
    player: Player,
}

#[rtic::app(device = lm3s6965, peripherals = true)]
impl GlobalControl {
    https://rust-embedded.github.io/book/peripherals/singletons.html
}

trait GlobalControlActions {

}

impl GlobalControlActions for GlobalControl {

}