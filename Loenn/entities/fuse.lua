local utils = require("utils")
local drawableLine = require("structs.drawable_line")
local drawableSprite = require("structs.drawable_sprite")

local fuse = {}

fuse.name = "HamburgerHelper/Fuse"
fuse.justification = {0.5, 0.5}

fuse.nodeRenderType = "false"
fuse.nodeLimits = {1, -1}
fuse.depth = -13001

fuse.placements = {
    {
        name = "fuse",
        data = {
            fuseSpeed = 3,
            playerIgnite = true,
            fuseColor = "a65959",
        }
    }
}

fuse.fieldInformation = {
    fuseColor = {
        fieldType = "color",
    },
}

local fuseTail = "objects/hamburger/firework/fuseEnd00"
local playerIgniteColor = {0.651, 0.2, 0.2, 1}
local fuseChainColor = {0.2, 0.651, 0.345, 1}

function fuse.nodeSprite() end

function fuse.sprite(room, entity, viewport)
    local fuseColor = entity.fuseColor or "a65959"

    local points = { entity.x, entity.y }
    for _, node in ipairs(entity.nodes or {}) do
        table.insert(points, node.x)
        table.insert(points, node.y)
    end

    local line = drawableLine.fromPoints(points, fuseColor, 1)

    local startX = entity.x
    local startY = entity.y
    local nextX = entity.nodes[1].x
    local nextY = entity.nodes[1].y

    local angle = math.atan(nextY - startY, nextX - startX)

    local fuseTailSprite = drawableSprite.fromTexture(fuseTail, entity)
    fuseTailSprite.rotation = angle

    local fuseTailColor = entity.playerIgnite and playerIgniteColor or fuseChainColor
    fuseTailSprite.color = fuseTailColor

    return { line, fuseTailSprite }
end

function fuse.selection(room, entity)
    local main = utils.rectangle(entity.x - 4, entity.y - 4, 8, 8)

    if entity.nodes then
        local nodeSelections = {}
        for _,node in ipairs(entity.nodes) do
            table.insert(nodeSelections, utils.rectangle(node.x - 4, node.y - 4, 8, 8))
        end
        return main, nodeSelections
    end

    return main, { }
end

return fuse