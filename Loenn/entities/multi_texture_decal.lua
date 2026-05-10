local utils = require("utils")

local entity = {}

entity.name = "HamburgerHelper/MultiTextureDecal"
entity.justification = {0.5, 0.5}

entity.placements = {
    {
        name = "fg_multi_texture_decal",
        data = {
            textures = "_fallback",
            scaleX = 1,
            scaleY = 1,
            rotation = 0,
            color = "ffffff",
            foreground = true,
            depthOffset = 0,
        }
    },
    {
        name = "bg_multi_texture_decal",
        data = {
            textures = "_fallback",
            scaleX = 1,
            scaleY = 1,
            rotation = 0,
            color = "ffffff",
            foreground = false,
            depthOffset = 0,
        }
    }
}

entity.fieldInformation = {
    textures = {
        fieldType = "list",
        elementOptions = {
            fieldType = "string",
        },
    },
    color = {
        fieldType = "color",
    },
    depthOffset = {
        fieldType = "integer",
    },
}


entity.fieldOrder = {"x", "y", "scaleX", "scaleY", "texture", "depth", "rotation", "color"}

local function getFirstTexture(textures)
    return textures:match("[^,]+") or "_fallback"
end

function entity.texture(room, entity)
    return "decals/" .. getFirstTexture(entity.textures)
end

function entity.rotation(room, entity)
    return math.rad(entity.rotation or 0)
end

function entity.depth(room, entity, viewport)
    local fgDepth = -10500
    local bgDepth = 9000
    local appliedDepth = entity.foreground and fgDepth or bgDepth

    local realDepth = appliedDepth + (entity.depthOffset or 0)
    return realDepth
end

function entity.flip(room, entity, horizontal, vertical)
    if (horizontal) then
        entity.scaleX = entity.scaleX * -1
        return true
    end

    if (vertical) then
        entity.scaleY = entity.scaleY * -1
        return true
    end

    return false
end

function entity.rotate(room, entity, direction)
    if direction ~= 0 then
        entity.rotation = ((entity.rotation or 0) + direction * 90) % 360
    end

    return direction ~= 0
end

-- i will add resize only if someone asks for it, OR i get a dm from scraggly1

return entity