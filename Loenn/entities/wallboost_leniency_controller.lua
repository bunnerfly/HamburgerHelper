local controller = {}

controller.name = "HamburgerHelper/WallboostLeniencyController"
controller.justification = {0.5, 0.5}

controller.placements = {
    {
        name = "wallboost_leniency_controller",
        data = {
            wallboostFrames = 12,
        }
    }
}

controller.texture = "loenn/hamburger/wallboostLeniencyController"

return controller