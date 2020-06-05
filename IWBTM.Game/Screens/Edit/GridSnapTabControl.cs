using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Edit
{
    public class GridSnapTabControl : EditorTabControl<int>
    {
        public GridSnapTabControl()
        {
            AddItem(32);
            AddItem(16);
            AddItem(8);
            AddItem(4);
            AddItem(2);
            AddItem(1);
        }

        protected override EditorTabItem<int> CreateItem(int value) => new GridSnapTabItem(value);

        private class GridSnapTabItem : EditorTabItem<int>
        {
            public GridSnapTabItem(int value)
                : base(value)
            {
                Add(new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = value.ToString(),
                    Colour = Color4.Black
                });
            }
        }
    }
}
