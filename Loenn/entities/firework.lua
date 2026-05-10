local utils = require("utils")
local drawableSprite = require("structs.drawable_sprite")

local firework = {}

firework.name = "HamburgerHelper/Firework"
firework.justification = {0.5, 0.5}
firework.depth = -13001

firework.placements = {
    {
        name = "firework",
        data = {
            direction = 1,
            launchTime = 0.5,
            launchSpeed = 2,
            fireworkSprite = "launchFirework",
            sidesOnly = false,
            snapUp = false,
        }
    }
}

local directions = {
    {"Up", 0},
    {"Down", 1},
    {"Left", 2},
    {"Right", 3},
}

function firework.fieldInformation()
    return {
        direction = {
            fieldType = "integer",
            options = directions,
            editable = false,
        },
    }
end

function firework.texture(room, entity) 
    return "objects/hamburger/firework/launchFirework"
end

local indicatorPath = "objects/hamburger/firework/indicator"
function firework.sprite(room, entity, viewport)
    local texture = firework.texture(room, entity)

    local sprite = drawableSprite.fromTexture(texture, entity)
    sprite.rotation = firework.rotation(room, entity)

    local sprites = { sprite }

    local direction = entity.direction or 0
    local launchTime = entity.launchTime or 0.5
    local launchSpeed = entity.launchSpeed or 2

    local dx, dy = 0, 0

    if direction == 0 then
        dy = -1
    elseif direction == 1 then
        dy = 1
    elseif direction == 2 then
        dx = -1
    elseif direction == 3 then
        dx = 1
    end

    local distance = launchSpeed * launchTime * 60

    local indicatorX = entity.x + dx * distance
    local indicatorY = entity.y + dy * distance

    -- somehow it doesn't complain as long as i only give it x and y
    local indicatorPosition = {
        x = indicatorX,
        y = indicatorY,
        depth = entity.depth
    }

    local indicator = drawableSprite.fromTexture(indicatorPath, indicatorPosition)
    indicator.color = {0.36, 0.36, 0.36, 1}

    table.insert(sprites, indicator)

    return sprites
end

function firework.rotation(room, entity)
    local direction = entity.direction or 0

    if direction == 0 then
        return math.rad(-90)
    elseif direction == 1 then
        return math.rad(90)
    elseif direction == 2 then
        return math.rad(180)
    elseif direction == 3 then
        return 0
    end

    return 0
end

function firework.selection(room, entity)
    return utils.rectangle(entity.x - 8, entity.y - 8, 16, 16)
end

return firework