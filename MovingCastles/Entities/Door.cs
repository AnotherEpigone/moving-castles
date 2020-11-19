﻿using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Fonts;
using MovingCastles.Maps;
using MovingCastles.Serialization.Entities;
using Newtonsoft.Json;
using SadConsole;
using System.Diagnostics;

namespace MovingCastles.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(DoorJsonConverter))]
    public class Door : McEntity
    {
        private const string OpenAnimationKey = "DoorOpen";
        private const string ClosedAnimationKey = "DoorClose";

        public Door(Coord position, Font font)
            : this(position, font, false) { }

        public Door(Coord position, Font font, bool open)
            : base("Door",
                  Color.White,
                  Color.Transparent,
                  SpriteAtlas.Door_Wood_Closed,
                  position,
                  (int)DungeonMapLayer.DOODADS,
                  isWalkable: false,
                  isTransparent: false,
                  Color.SaddleBrown)
        {
            // door open/close animations
            var closedAnimation = new AnimatedConsole(ClosedAnimationKey, 1, 1, font);
            closedAnimation.CreateFrame().SetGlyph(0, 0, SpriteAtlas.Door_Wood_Closed, Color.White, Color.Transparent);
            var openAnimation = new AnimatedConsole(OpenAnimationKey, 1, 1, font);
            openAnimation.CreateFrame().SetGlyph(0, 0, SpriteAtlas.Door_Wood_Open, Color.White, Color.Transparent);
            Animations.Clear();
            Animations.Add(ClosedAnimationKey, closedAnimation);
            Animations.Add(OpenAnimationKey, openAnimation);
            Animation = Animations[ClosedAnimationKey];

            AddGoRogueComponent(new OpenDoorComponent());
            AddGoRogueComponent(new UseDoorComponent());

            // workaround Entity construction bugs by setting font afterward
            Font = font;
            OnCalculateRenderPosition();

            IsOpen = false;

            if (open)
            {
                Open();
            }
        }

        public bool IsOpen { get; private set; }

        public void Open()
        {
            Animation = Animations[OpenAnimationKey];
            IsWalkable = true;
            IsTransparent = true;
            IsOpen = true;
        }

        public void Toggle()
        {
            IsOpen = !IsOpen;
            Animation = IsOpen
                ? Animations[OpenAnimationKey]
                : Animations[ClosedAnimationKey];
            IsWalkable = IsOpen;
            IsTransparent = IsOpen;
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(Door)}: {Name}");
            }
        }
    }
}