using System;
using System.Drawing;
using System.Windows.Forms;

public class DialogTestSkin : SkinnedWindow
{
    SkinnedButton button;
    SkinnedButton button2;
    SkinnedLabel label;
    SkinnedMenuStrip menuStrip;
    SkinnedPanel panel;
    SkinnedTextBox textBox;
    SkinnedToolStripItem menuStripItem1;
    SkinnedToolStripItem menuStripItem2;
    SkinnedToolStripItem menuStripItemItem1;
    SkinnedToolStripItem menuStripItemItem2;
    SkinnedToolStripItem menuStripItemItem3;
    SkinnedToolStripItem menuStripItemItemItem1;
    SkinnedToolStripItem menuStripItemItemItem2;
    SkinnedToolStripItem menuStripItemItemItem3;

    TextBoxMarked textBoxMarked;
    TextBoxMarked textBoxNumberMarked;

    LayerList layerList;

    public DialogTestSkin()
    {
        this.Size = new Size(400, 310);

        label = new SkinnedLabel();

        menuStrip = new SkinnedMenuStrip();
        menuStripItem1 = new SkinnedToolStripItem();
        menuStripItem2 = new SkinnedToolStripItem();
        menuStripItemItem1 = new SkinnedToolStripItem();
        menuStripItemItem2 = new SkinnedToolStripItem();
        menuStripItemItem3 = new SkinnedToolStripItem();
        menuStripItemItemItem1 = new SkinnedToolStripItem();
        menuStripItemItemItem2 = new SkinnedToolStripItem();
        menuStripItemItemItem3 = new SkinnedToolStripItem();

        panel = new SkinnedPanel();
        textBox = new SkinnedTextBox();
        textBoxMarked = new TextBoxMarked("Type Text...");
        textBoxNumberMarked = new TextBoxMarked("Type Number...");
        button = new SkinnedButton();
        button2 = new SkinnedButton();

        layerList = new LayerList(150, 200);

        // set values
        label.Location = new Point(20, 30);
        menuStrip.Location = new Point(0, 0);

        panel.Location = new Point(200, 30);
        textBox.Location = new Point(50, 10);
        textBoxMarked.Location = new Point(50, 35);
        textBoxNumberMarked.Location = new Point(50, 60);
        button.Location = new Point(50, 100);
        button2.Location = new Point(50, 140);

        layerList.Location = new Point(20, 60);

        layerList.LayerItemQuickAdd += (s, e) =>
        {
            LayerList list = s as LayerList;
            list.AddItem(null);
        };
        layerList.LayerItemAdd += (s, e) =>
        {
            string str = textBox.Text;
            layerList.AddItem(str);
        };
        layerList.LayerItemRemove += (s, e) =>
        {
            int i = layerList.m_selectedItem;
            if(i >= 0)
                layerList.removeItem(i, e);
            else
                Console.Write("\nClick this to delete the selected layer.");
        };

        button.MouseUp += (s, e) =>
        {
            Console.Write("\nThis button does nothing.");
        };

        button2.MouseUp += (s, e) =>
        {
                Console.Write("\npoke.");
        };

        layerList.LayerItemsChanged += (s, e) =>
        {
            int selected = layerList.m_selectedItem;

            if (selected < 0)
                return;

            label.Text = selected.ToString();
        };















        // add controls
        menuStripItemItem1.DropDownItems.Add(menuStripItemItemItem1);
        menuStripItemItem1.DropDownItems.Add(menuStripItemItemItem2);
        menuStripItemItem1.DropDownItems.Add(menuStripItemItemItem3);
        menuStripItem1.DropDownItems.Add(menuStripItemItem1);
        menuStripItem1.DropDownItems.Add(menuStripItemItem2);
        menuStripItem1.DropDownItems.Add(menuStripItemItem3);
        menuStrip.Items.Add(menuStripItem1);
        menuStrip.Items.Add(menuStripItem2);

        panel.Controls.Add(textBox);
        panel.Controls.Add(textBoxMarked);
        panel.Controls.Add(textBoxNumberMarked);
        panel.Controls.Add(button);
        panel.Controls.Add(button2);

        this.Controls.Add(label);
        this.Controls.Add(panel);
        this.Controls.Add(layerList);
        this.Controls.Add(menuStrip);
    }
}
