local entity = {}

entity.name = "HamburgerHelper/DreamerRefill"
entity.justification = {0.5, 0.5}

entity.placements = {
    {
        name = "dreamer_refill",
        data = {
            oneUse = false,
            respawnTime = 2.5,
        }
    }
}

entity.texture = "objects/hamburger/dreamerrefill/idle00"

return entity