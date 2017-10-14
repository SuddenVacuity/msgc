using System;
using System.Windows.Forms;
using System.Drawing;

public class LayerList : Control
{
    Panel m_header;
    Panel m_body;
    Panel m_footer;

    public event EventHandler LayerItemsChanged;
    public event EventHandler LayerItemQuickAdd;
    public event EventHandler LayerItemAdd;
    public event EventHandler LayerItemRemove;

    public event EventHandler LayerItemButtonVisibilityClick;

    public int m_selectedItem = -1;
    private int m_totalItemsAdded = 0;

    public LayerList()
    {
        // these need to be initialized right away
        // so they are valid when OnSizeChanged event triggers
        m_header = new Panel();
        m_footer = new Panel();
        m_body = new Panel();

        this.Size = InterfaceSkin.LayerListSize;

        setMainControlValues();
        populateFooter();

        this.Controls.Add(m_header);
        this.Controls.Add(m_footer);
        this.Controls.Add(m_body);
    }

    public LayerList(int width, int height)
    {
        // these need to be initialized right away
        // so they are valid when OnSizeChanged event triggers
        m_header = new Panel();
        m_footer = new Panel();
        m_body = new Panel();

        this.Size = new Size(width, height);

        setMainControlValues();
        populateFooter();

        this.Controls.Add(m_header);
        this.Controls.Add(m_footer);
        this.Controls.Add(m_body);
    }

    /// <summary>
    /// Sets the values of the list's main controls
    /// </summary>
    private void setMainControlValues()
    {
        m_header.Size = new Size(this.Size.Width, InterfaceSkin.LayerListHeaderHeight);
        m_header.BackColor = InterfaceSkin.LayerListHeaderColorBackground;
        m_header.ForeColor = InterfaceSkin.LayerListHeaderColorText;

        m_footer.Size = new Size(this.Size.Width, InterfaceSkin.LayerListFooterHeight);
        m_footer.BackColor = InterfaceSkin.LayerListFooterColorBackground;

        m_body.HorizontalScroll.Maximum = 0;
        m_body.AutoScroll = false;
        m_body.VerticalScroll.Visible = false;
        m_body.AutoScroll = true;
        m_body.Location = new Point(0, m_header.Size.Height);
        m_body.Size = new Size(this.Size.Width, this.Size.Height - (m_header.Size.Height + m_footer.Size.Height));
        m_body.BackColor = InterfaceSkin.LayerListBodyColorBackground;
        m_body.ForeColor = InterfaceSkin.LayerListBodyColorText;

        m_footer.Location = new Point(0, m_header.Size.Height + m_body.Size.Height);
    }

    /// <summary>
    /// Add interface controls to the footer
    /// </summary>
    private void populateFooter()
    {
        Label quickLayer = new Label();
        quickLayer.Text = "+";
        quickLayer.Size = new Size(21, 21);
        quickLayer.Location = new Point(5, (m_footer.Height - quickLayer.Height) / 2);
        quickLayer.BackColor = InterfaceSkin.LayerListButtonColorBackground;
        quickLayer.ForeColor = InterfaceSkin.LayerListButtonColorText;
        quickLayer.TextAlign = ContentAlignment.MiddleCenter;
        quickLayer.MouseUp += (s, e) =>
        {
            // run event for parent object
            EventHandler handler = LayerItemQuickAdd;
            if (handler != null)
                handler(this, e);
        };

        Label removeLayer = new Label();
        removeLayer.Text = "-";
        removeLayer.Size = new Size(21, 21);
        removeLayer.Location = new Point(this.Size.Width - (5 + removeLayer.Size.Width), (m_footer.Height - removeLayer.Height) / 2);
        removeLayer.BackColor = InterfaceSkin.LayerListButtonColorBackground;
        removeLayer.ForeColor = InterfaceSkin.LayerListButtonColorText;
        removeLayer.TextAlign = ContentAlignment.MiddleCenter;
        removeLayer.MouseUp += (s, e) =>
        {
            // run event for parent object
            EventHandler handler = LayerItemRemove;
            if (handler != null)
                handler(this, e);
        };
        
        Label addLayer = new Label();
        addLayer.Text = "create layer";
        addLayer.Size = new Size(removeLayer.Location.X - (quickLayer.Location.X + quickLayer.Size.Width) - 10, 21);
        addLayer.Location = new Point(quickLayer.Location.X + quickLayer.Size.Width + 5,
            (m_footer.Height - addLayer.Height) / 2);
        addLayer.BackColor = InterfaceSkin.LayerListButtonColorBackground;
        addLayer.ForeColor = InterfaceSkin.LayerListButtonColorText;
        addLayer.TextAlign = ContentAlignment.BottomCenter;
        addLayer.MouseUp += (s, e) =>
        {
            // run event for parent object
            EventHandler handler = LayerItemAdd;
            if (handler != null)
                handler(this, e);
        };

        m_footer.Controls.Add(quickLayer);
        m_footer.Controls.Add(removeLayer);
        m_footer.Controls.Add(addLayer);
    }
    
    /// <summary>
    /// Adds and entry to the end of the list
    /// </summary>
    /// <param name="text">The text that will display in the list</param>
    public void AddItem(string text)
    {
        // used to track id and calculate position
        // the current item count must be set to the list item's panel name
        int count = m_body.Controls.Count;

        ListItem item = new ListItem();
        item.m_id = count;

        // check if a default name needs to be made
        if (text == null)
            item.m_text = "Layer " + (m_totalItemsAdded + 1);
        else
            item.m_text = text;

        // add a mouse down event to the item being added
        item.MouseDown += (s, e) =>
        {
            // treat sender as ListItem to be able to 
            // access ListItem elements
            ListItem clicked = s as ListItem;
            
            if (m_selectedItem >= 0)
            {
                ListItem previous = m_body.Controls[m_selectedItem] as ListItem;
                previous.deselect();
            }
            
            // turn the string id to an int id
            //int.TryParse(clicked.Name, out m_selectedItem);
            m_selectedItem = clicked.m_id;

            // run event for parent object
            EventHandler handler = LayerItemsChanged;
            if (handler != null)
                handler(this, e);
        };
        item.ListItemButtonClick += (s, e) =>
        {
            // run event for parent object
            EventHandler handler = LayerItemButtonVisibilityClick;
            if (handler != null)
                handler(item, e);
        };
        
        // set the items location based on it's id
        Point loc = new Point(0, count * item.Size.Height - m_body.VerticalScroll.Value);
        item.Location = loc;

        m_body.Controls.Add(item);
        m_totalItemsAdded += 1;
    }

    /// <summary>
    /// Returns true if removal was successful. 
    /// Removes the item at index and shifts all after index to the left.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="e">Eventargs required to listen for event</param>
    /// <returns></returns>
    public bool removeItem(int index, EventArgs e)
    {
        // check that index and control.count are valid
        if (index < 0 ||
            index >= m_body.Controls.Count ||
            m_body.Controls.Count < 1)
            return false;

        // adjust the selected item index to match what the index will be after shifting
        if (index == m_selectedItem)
            m_selectedItem = -1;
        if (index < m_selectedItem)
            m_selectedItem -= 1;

        // remove the item from controls
        m_body.Controls.RemoveAt(index);

        // set id and location for each item after the removed item to match it's new index
        for (int i = index; i < m_body.Controls.Count; i++)
        {
            ListItem item = m_body.Controls[i]as ListItem;
            item.m_id = i;

            item.Location = new Point(
                0, 
                i * item.Size.Height - m_body.VerticalScroll.Value);
        }

        // run event for parent object
        EventHandler handler = LayerItemsChanged;
        if (handler != null)
            handler(this, e);

        return true;
    }

    /// <summary>
    /// Resets selection and total items added then clears all controls from the body.
    /// </summary>
    public void clearAll()
    {
        m_selectedItem = -1;
        m_totalItemsAdded = 0;
        m_body.Controls.Clear();
    }
    /// <summary>
    /// resize then repopulate all the main controls
    /// </summary>
    /// <param name="e">Eventargs required for event system</param>
    protected override void OnSizeChanged(EventArgs e)
    {
        m_header.Size = new Size(this.Size.Width, InterfaceSkin.LayerListHeaderHeight);
        m_footer.Size = new Size(this.Size.Width, InterfaceSkin.LayerListFooterHeight);
        m_body.Size = new Size(this.Size.Width, this.Size.Height - (m_header.Size.Height + m_footer.Size.Height));
        m_footer.Location = new Point(0, m_header.Size.Height + m_body.Size.Height);

        m_footer.Controls.Clear();
        populateFooter();

        base.OnSizeChanged(e);
    }
}
