local utils = require("utils")

local entity = {}

entity.name = "HamburgerHelper/LighterRefill"
entity.justification = {0.5, 0.5}

entity.placements = {
    {
        name = "lighter",
        data = {
            oneUse = false,
            respawnTime = 2.5,
        }
    }
}

entity.texture = "objects/hamburger/lighter/idle00"

function entity.rectangle(room, entity)
    return utils.rectangle(entity.x - 5, entity.y - 5, 10, 10)
end

return entity