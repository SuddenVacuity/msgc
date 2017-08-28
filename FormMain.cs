using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace msgc
{
    

    public partial class FormMain : Form
    {
        public class BitField
        {
            private int m_flags;

            public BitField()
            {
                m_flags = 0;
            }
            public BitField(int flags)
            {
                m_flags = flags;
            }

            public int get() { return m_flags; }
            public void set(int flags) { m_flags = flags; }
            public void add(int flags) { m_flags |= flags; }
            public void remove(int flags) { m_flags -= (m_flags & flags); }
            public bool has(int flags) { return (m_flags & flags) == flags; }
        }

        public enum BlendMode
        {
            None,
            DrawAdd,
            DrawMix,
            Replace,
            Erase,
        }

        // stores canvas fragments and blending data
        private class Layer
        {
            private BitField m_flags = new BitField();
            private int m_id = int.MinValue;
            private string m_name = "";
            private CanvasFragment m_fragment;
            private bool m_isVisible = true;
            //Vector
            //Filter
            
            public Layer(int id, string name, CanvasFragment fragment, int flags = 0)
            {
                m_flags.set(flags);
                m_id = id;
                m_name = name;
                m_fragment = fragment;
                m_isVisible = true;
            }
            public void clearLayer()
            {
                m_id = int.MinValue;
                m_name = "";
                m_fragment.clear();
            }

            public void resizeImage(Point location, Size size)
            {
                Point currentLocation = m_fragment.getPosition();
                Point offset = new Point(0, 0);

                if (location.X < currentLocation.X)
                    offset.X = currentLocation.X - location.X;
                if (location.Y < currentLocation.Y)
                    offset.Y = currentLocation.Y - location.Y;
                
                m_fragment.setPosition(location.X, location.Y);
                m_fragment.resize(size.Width, size.Height, offset);
            }

            public void setImage(Bitmap image) { m_fragment.setImage(image); }
            public Bitmap getImage() { return m_fragment.getImage();  }
            public int getId() { return m_id; }
            public void setId(int id) { m_id = id; }
            public void setIsVisible(bool visible) { m_isVisible = visible; }
            public bool getIsVisible() { return m_isVisible; }
            public Color getPixel(int x, int y) { return m_fragment.getPixel(x, y); }
            public Color getPixel(Point point)  { return m_fragment.getPixel(point.X, point.Y); }
            public void setPixel(int x, int y, Color color) { m_fragment.setPixel(x, y, color); }
            public void setPixel(Point point, Color color)  { m_fragment.setPixel(point.X, point.Y, color); }
            public Rectangle getRectangle() { return new Rectangle(m_fragment.getPosition(), m_fragment.getSize()); }

            public void addFlag(int flag) { m_flags.add(flag); }
            public void removeFlag(int flag) { m_flags.remove(flag); }
            public bool hasFlag(int flag) { return m_flags.has(flag); }
        }
        // stores an image, a location and data about the image
        private class CanvasFragment
        {
            private BitField m_flags = new BitField();
            private Bitmap m_image = null;

            // distance from left side and top
            private int m_x = int.MinValue;
            private int m_y = int.MinValue;

            public CanvasFragment(Bitmap image, int x, int y, int flags = 0)
            {
                m_flags.set(flags);
                setPosition(x, y);
                setImage(image);
            }
            public CanvasFragment(Bitmap image, int flags = 0)
            {
                m_flags.set(flags);
                setImage(image);
            }

            ~CanvasFragment()
            {
                if(m_image != null)
                    m_image.Dispose();
            }

            public void clear()
            {
                m_flags = new BitField();
                if (m_image != null)
                    m_image.Dispose();
                m_x = int.MinValue;
                m_y = int.MinValue;
            }

            public void setImage(Bitmap image)
            {
                if (m_image != null)
                    m_image.Dispose();

                if(image != null)
                    m_image = (Bitmap)image.Clone();
            }
            public Color getPixel(int x, int y)
            {
                if (m_image == null)
                    return Color.Transparent;

                int posX = x - m_x;
                int posY = y - m_y;

                if (posX < 0 || posY < 0)
                    return Color.FromArgb(0, 0, 0, 0);
                if(posX >= m_image.Width || posY >= m_image.Height)
                    return Color.FromArgb(0, 0, 0, 0);

                return m_image.GetPixel(posX, posY);
            }
            public void setPixel(int x, int y, Color color)
            {
                if (m_image == null)
                    return;

                int posX = x - m_x;
                int posY = y - m_y;

                if (posX < 0 || posY < 0)
                    return;
                if (posX >= m_image.Width || posY >= m_image.Height)
                    return;

                m_image.SetPixel(posX, posY, color);
            }
            public void resize(int width, int height, Point imageOffset)
            {
                if (m_image == null)
                    return;

                Bitmap old = m_image;
                m_image = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                using (Graphics gr = Graphics.FromImage(m_image))
                {
                    gr.DrawImageUnscaled(old, imageOffset);
                    gr.Save();
                }
                old.Dispose();
            }

            public void setPosition(int x, int y) { m_x = x; m_y = y; }
            public Size getSize() { return m_image.Size; }
            public Point getPosition() { return new Point(m_x, m_y); }
            public Bitmap getImage() { return m_image; }

            public void addFlag(int flag) { m_flags.add(flag); }
            public void removeFlag(int flag) { m_flags.remove(flag); }
            public bool hasFlag(int flag) { return m_flags.has(flag); }
        }

        // default values
        private BlendMode m_defaultBlendMode = BlendMode.DrawAdd;
        private Color m_defaultBrushColor = Color.FromArgb(255, 255, 255, 255);
        private Color m_defaultColor = Color.Black;
        private Color m_defaultColorAlt = Color.White;
        private string m_defaultCanvasImage = @"//colorTest.png";
        private string m_defaultBrush = @"//standard_round.png";


        ////////////////////
        // layer tools
        ////////////////////
        private int m_layerCount;
        private int m_layerCurrent;
        private int m_maxLayers = 255;
        private Layer[] m_layers;
        // the image used to store a flattened image that's shown in display_canvas
        private Bitmap m_canvasImage;
        // the image that will be saved to file if saved
        private Bitmap m_finalImage;

        ////////////////////
        // drawing tools
        ////////////////////
        // the position the mouse was last mousemove update
        private Point m_mousePosPrev;
        private bool m_isDrawing;
        // where drawing input is stored bfore it's flattened to m_finalImage
        private Bitmap m_drawBuffer;
        // region where m_drawBuffer has been drawn on
        private Rectangle m_redrawDrawBufferZone;
        
        private Color m_color;
        private Color m_colorAlt;
        private Bitmap m_brush;
        private BlendMode m_brushMode;



        /*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        / %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        /                          FUNCTIONS
        / %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        / %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*/

        /// <summary>
        // initializes the main form and objects in the heap
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.AllowTransparency = true;

            // initialize
            m_layers = new Layer[m_maxLayers];

            string dir = Environment.CurrentDirectory + m_defaultCanvasImage;
            if (System.IO.File.Exists(dir))
            {
                m_canvasImage = new Bitmap(dir);
            }
            else
            {
                Console.Write("\nDefault image not found: " + dir + "\nCreating new image");
                Bitmap img = new Bitmap(800, 600, PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(img))
                {
                    gr.Clear(Color.White);
                }
                m_canvasImage = img;
            }

            m_finalImage = new Bitmap(m_canvasImage);
            display_canvas.Image = (Bitmap)m_canvasImage.Clone();

            m_color = m_defaultColor;
            m_colorAlt = m_defaultColorAlt;

            m_brushMode = m_defaultBlendMode;
            button_round_brush.BackColor = Color.Black;

            // load brush
            dir = Environment.CurrentDirectory + m_defaultBrush;
            if (System.IO.File.Exists(dir))
            {
                m_brush = new Bitmap(dir);
                for (int i = 0; i < m_brush.Size.Height; i++)
                    for (int j = 0; j < m_brush.Size.Width; j++)
                    {
                        Color c = m_brush.GetPixel(j, i);
                        Color inverted = Color.FromArgb(
                            c.A,
                            byte.MaxValue - c.R,
                            byte.MaxValue - c.G,
                            byte.MaxValue - c.B);
                        m_brush.SetPixel(j, i, inverted);
                    }
            }
            else // load failed - create new bursh
            {
                Console.Write("\nDefault brush not found: " + dir + "\nCreating new brush");
                Bitmap img = new Bitmap(30, 30, PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(img))
                {
                    gr.Clear(Color.Transparent);
                }
                m_brush = img;
                for (int i = 0; i < m_brush.Size.Height; i++)
                    for (int j = 0; j < m_brush.Size.Width; j++)
                    {
                        if (i < 3 || j < 3)
                            continue;
                        if (i > m_brush.Height - 3 || j > m_brush.Width - 3)
                            continue;

                        if (i - j == 0)
                        {
                            m_brush.SetPixel(j, i, Color.Black);
                            m_brush.SetPixel(j + 1, i, Color.Black);
                            m_brush.SetPixel(j, i + 1, Color.Black);
                            m_brush.SetPixel(j - 1, i, Color.Black);
                            m_brush.SetPixel(j, i - 1, Color.Black);
                        }
                        if (m_brush.Width - j == i)
                        {
                            m_brush.SetPixel(j, i, Color.Black);
                            m_brush.SetPixel(j + 1, i, Color.Black);
                            m_brush.SetPixel(j, i + 1, Color.Black);
                            m_brush.SetPixel(j - 1, i, Color.Black);
                            m_brush.SetPixel(j, i - 1, Color.Black);
                        }
                    }
            }

            m_isDrawing = false;
            m_redrawDrawBufferZone = new Rectangle(0, 0, 0, 0);

            // make canves the correct size and create image
            display_canvas.Size = m_canvasImage.Size;
            m_drawBuffer = new Bitmap(display_canvas.Image.Size.Width, display_canvas.Image.Size.Height, PixelFormat.Format32bppArgb);

            // load layers
            m_layers[m_layerCount] = new Layer(m_layerCount, "Background", new CanvasFragment((Bitmap)display_canvas.Image.Clone(), 0, 0));
            m_layerCurrent = m_layers[m_layerCount].getId();
            m_layerCount++;
            m_layers[m_layerCount] = new Layer(m_layerCount, "Layer 2", new CanvasFragment(new Bitmap(display_canvas.Image.Width, display_canvas.Image.Height, PixelFormat.Format32bppArgb), 0, 0));
            m_layerCurrent = m_layers[m_layerCount].getId();
            m_layerCount++;
            m_layers[m_layerCount] = new Layer(m_layerCount, "Layer 3", new CanvasFragment(new Bitmap(display_canvas.Image.Width, display_canvas.Image.Height, PixelFormat.Format32bppArgb), 0, 0));
            m_layerCurrent = m_layers[m_layerCount].getId();
            m_layerCount++;

            // set up color boxes
            layer_1.BackColor = Color.Transparent;
            layer_2.BackColor = Color.Transparent;
            layer_3.BackColor = Color.Black;
            layer_1_visible.BackColor = Color.Black;
            layer_2_visible.BackColor = Color.Black;
            layer_3_visible.BackColor = Color.Black;

            text_input_color_alpha.Text = Convert.ToString(m_color.A);
            text_input_color_red.Text = Convert.ToString(m_color.R);
            text_input_color_green.Text = Convert.ToString(m_color.G);
            text_input_color_blue.Text = Convert.ToString(m_color.B);
            
            color_box.BackColor = m_color;
            color_box_alt.BackColor = m_colorAlt;
            color_black.BackColor = Color.Black;
            color_gray.BackColor = Color.Gray;
            color_white.BackColor = Color.White;
            color_1.BackColor = Color.FromArgb(255, 255, 0,   0);
            color_2.BackColor = Color.FromArgb(255, 255, 127, 0);
            color_3.BackColor = Color.FromArgb(255, 255, 255, 0);
            color_4.BackColor = Color.FromArgb(255, 127, 255, 0);

            color_5.BackColor = Color.FromArgb(255, 0, 255, 0);
            color_6.BackColor = Color.FromArgb(255, 0, 255, 127);
            color_7.BackColor = Color.FromArgb(255, 0, 255, 255);
            color_8.BackColor = Color.FromArgb(255, 0, 127, 255);

            color_9.BackColor  = Color.FromArgb(255, 0,   0, 255);
            color_10.BackColor = Color.FromArgb(255, 127, 0, 255);
            color_11.BackColor = Color.FromArgb(255, 255, 0, 255);
            color_12.BackColor = Color.FromArgb(255, 255, 0, 127);
            
        }
        
        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// get and act on key event input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            int key = e.KeyChar;

            // shift lowercase letter to uppercase
            // assumes lowercase (char) have higher values than uppercase
            if (key >= 'a' && key <= 'z')
                key -= ('a' - 'A');

            bool isShiftPressed = (Control.ModifierKeys == Keys.Shift);
            bool isControlPressed = (Control.ModifierKeys == Keys.Control);

            switch (key)
            {
                default: break;
                //case (char)Keys.R: m_color = Color.FromArgb(255, 255, 0, 0); Console.Write("\nDraw color is now red"); break;
                //case (char)Keys.G: m_color = Color.FromArgb(255, 0, 255, 0); Console.Write("\nDraw color is now green"); break;
                //case (char)Keys.B: m_color = Color.FromArgb(255, 0, 0, 255); Console.Write("\nDraw color is now blue"); break;
                //case (char)Keys.D0: m_color = Color.FromArgb(0            , m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now 0%"); break;
                //case (char)Keys.D1: m_color = Color.FromArgb(1 * (255 / 9), m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now ~11%"); break;
                //case (char)Keys.D2: m_color = Color.FromArgb(2 * (255 / 9), m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now ~22%"); break;
                //case (char)Keys.D3: m_color = Color.FromArgb(3 * (255 / 9), m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now ~33%"); break;
                //case (char)Keys.D4: m_color = Color.FromArgb(4 * (255 / 9), m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now ~44%"); break;
                //case (char)Keys.D5: m_color = Color.FromArgb(5 * (255 / 9), m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now ~55%"); break;
                //case (char)Keys.D6: m_color = Color.FromArgb(6 * (255 / 9), m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now ~66%"); break;
                //case (char)Keys.D7: m_color = Color.FromArgb(7 * (255 / 9), m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now ~77%"); break;
                //case (char)Keys.D8: m_color = Color.FromArgb(8 * (255 / 9), m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now ~88%"); break;
                //case (char)Keys.D9: m_color = Color.FromArgb(255          , m_color.R, m_color.G, m_color.B); Console.Write("\nDraw Opacity is now 110%"); break;
            }
            Console.Write("\nA key was pressed");

        }

        //////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////
        //                                                              //
        //                      FORM ELEMENTS                           //
        //                                                              //
        //////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////

        /////////////////////////////////////
        //  Menu Strip
        /////////////////////////////////////

        // file
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create blank image
            Bitmap newImage = new Bitmap(m_finalImage.Width, m_finalImage.Height, PixelFormat.Format32bppArgb);

            // clear all persisting images
            if (m_finalImage != null)
                m_finalImage.Dispose();
            if (m_canvasImage != null)
                m_canvasImage.Dispose();
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();
            
            // set default image to new image
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.Clear(Color.White);
                gr.Save();
            }

            // assign image to persisting images
            m_finalImage = newImage;
            m_canvasImage = (Bitmap)newImage.Clone();
            display_canvas.Image = (Bitmap)newImage.Clone();

            // clear layers
            for (int i = 0; i < m_layerCount; i++)
            {
                m_layers[i].clearLayer();
            }
            m_layerCount = 0;

            // set first layer to default image
            m_layers[m_layerCount] = new Layer(m_layerCount, "BackGround", new CanvasFragment(new Bitmap(newImage), 0, 0));
            m_layerCurrent = m_layers[m_layerCount].getId();
            m_layerCount++;
            m_layers[m_layerCount] = new Layer(m_layerCount, "Layer 2", new CanvasFragment(new Bitmap(newImage.Width, newImage.Height, PixelFormat.Format32bppArgb), 0, 0));
            m_layerCurrent = m_layers[m_layerCount].getId();
            m_layerCount++;
            m_layers[m_layerCount] = new Layer(m_layerCount, "Layer 3", new CanvasFragment(new Bitmap(newImage.Width, newImage.Height, PixelFormat.Format32bppArgb), 0, 0));
            m_layerCurrent = m_layers[m_layerCount].getId();
            m_layerCount++;

            // update display
            display_canvas.Refresh();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_finalImage != null)
                {
                    m_finalImage.Save(Environment.CurrentDirectory + "\\output.bmp");
                    Console.Write("\nImage saved");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error saving the file.");
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // edit
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // view
        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void viewScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void viewToSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void elementsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // project
        private void modeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // select
        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void inverseSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void cropSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void shrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void transformSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void byColorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // layer

        // tools

        // window

        // help



        /////////////////////////////////////
        // tool buttons
        /////////////////////////////////////
        private void button_pencil_Click(object sender, EventArgs e)
        {
            m_brushMode = BlendMode.DrawAdd;
        }
        private void button_eraser_Click(object sender, EventArgs e)
        {
            m_brushMode = BlendMode.Erase;
        }

        /////////////////////////////////////
        // text input
        /////////////////////////////////////
        
        private int stringToByteValue(string s, int failValue)
        {
            int v = failValue;
            int.TryParse(s, out v);

            if (v > 255)
            {
                v = 255;
            }
            return v;
        }

        private void text_input_color_red_TextChanged(object sender, EventArgs e)
        {
            string s = text_input_color_red.Text;
            int v = stringToByteValue(s, m_color.R);

            m_color = Color.FromArgb(
                m_color.A,
                v,
                m_color.G,
                m_color.B);

            text_input_color_red.Text = Convert.ToString(v);
            color_box.BackColor = m_color;
        }
        private void text_input_color_green_TextChanged(object sender, EventArgs e)
        {
            string s = text_input_color_green.Text;
            int v = stringToByteValue(s, m_color.G);

            m_color = Color.FromArgb(
                m_color.A,
                m_color.R,
                v,
                m_color.B);

            text_input_color_green.Text = Convert.ToString(v);
            color_box.BackColor = m_color;
        }
        private void text_input_color_blue_TextChanged(object sender, EventArgs e)
        {
            string s = text_input_color_blue.Text;
            int v = stringToByteValue(s, m_color.B);

            m_color = Color.FromArgb(
                m_color.A,
                m_color.R,
                m_color.G,
                v);

            text_input_color_blue.Text = Convert.ToString(v);
            color_box.BackColor = m_color;
        }
        private void text_input_color_alpha_TextChanged(object sender, EventArgs e)
        {
            string s = text_input_color_alpha.Text;
            int v = stringToByteValue(s, m_color.A);

            m_color = Color.FromArgb(
                v,
                m_color.R,
                m_color.G,
                m_color.B);

            text_input_color_alpha.Text = Convert.ToString(v);
            color_box.BackColor = m_color;
        }


        /////////////////////////////////////
        // color selection
        /////////////////////////////////////

        private void updateColorInputTextBoxes(Color color)
        {
            m_color = color;
            color_box.BackColor = m_color;
            text_input_color_alpha.Text = Convert.ToString(m_color.A);
            text_input_color_red.Text = Convert.ToString(m_color.R);
            text_input_color_green.Text = Convert.ToString(m_color.G);
            text_input_color_blue.Text = Convert.ToString(m_color.B);
        }
        private void color_black_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_black.BackColor);
        }
        private void color_gray_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_gray.BackColor);
        }
        private void color_white_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_white.BackColor);
        }
        private void color_1_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_1.BackColor);
        }
        private void color_2_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_2.BackColor);
        }
        private void color_3_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_3.BackColor);
        }
        private void color_4_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_4.BackColor);
        }
        private void color_5_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_5.BackColor);
        }
        private void color_6_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_6.BackColor);
        }
        private void color_7_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_7.BackColor);
        }
        private void color_8_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_8.BackColor);
        }
        private void color_9_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_9.BackColor);
        }
        private void color_10_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_10.BackColor);
        }
        private void color_11_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_11.BackColor);
        }
        private void color_12_Click(object sender, EventArgs e)
        {
            updateColorInputTextBoxes(color_12.BackColor);
        }

        /////////////////////////////////////
        // canvas
        /////////////////////////////////////

        /// <summary>
        /// Runs when the mouse button is pressed. Sets m_isDrawing to true. 
        /// Gets current mouse position. 
        /// Sets m_redrawDrawBuffer to the current mouse position. 
        /// Draws to m_drawBuffer. 
        /// Flattens layers and buffer to display_canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void display_canvas_MouseDown(object sender, MouseEventArgs e)
        {
            m_isDrawing = true;
            
            // create draw buffer
            m_mousePosPrev = display_canvas.PointToClient(Cursor.Position);
            Console.Write("\nClick position within draw_canvas: " + m_mousePosPrev.X + ", " + m_mousePosPrev.Y);

            m_redrawDrawBufferZone.X = m_mousePosPrev.X - m_brush.Size.Width;
            m_redrawDrawBufferZone.Y = m_mousePosPrev.Y - m_brush.Size.Height;
            m_redrawDrawBufferZone.Width = m_brush.Size.Width * 2;
            m_redrawDrawBufferZone.Height = m_brush.Size.Height * 2;
            
            drawToBuffer(m_mousePosPrev);
            
            Bitmap final = new Bitmap(display_canvas.Image.Width, display_canvas.Image.Height, PixelFormat.Format32bppArgb);
            
            using (Graphics gr = Graphics.FromImage(final))
            {
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                
                gr.DrawImage(m_canvasImage,
                    new Rectangle(0, 0, m_canvasImage.Width, m_canvasImage.Height),
                    new Rectangle(0, 0, m_canvasImage.Width, m_canvasImage.Height),
                    GraphicsUnit.Pixel);
                gr.DrawImage(m_drawBuffer,
                    m_redrawDrawBufferZone,
                    m_redrawDrawBufferZone,
                    GraphicsUnit.Pixel);
                gr.Save();
            }

            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            display_canvas.Image = final;
            drawUI();
            display_canvas.Refresh();
        }

        /// <summary>
        /// Runs whenever the mouse moves. 
        /// If m_isDrawing is true: 
        ///    Gets the current mouse position. 
        ///    Draws to m_drawBuffer. 
        ///    Flattens layers and buffer to display_canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void display_canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //return;
            if (!m_isDrawing)
                return;
            
            // current mouse position
            Point mousePos = display_canvas.PointToClient(Cursor.Position);
            
            drawToBuffer(mousePos);
            
            Point cursorCenter = new Point(
                mousePos.X - m_brush.Size.Width / 2,
                mousePos.Y - m_brush.Size.Height / 2);

            //flattenImage(m_canvasImage, m_redrawDrawufferZone);
            Bitmap final = new Bitmap(display_canvas.Image.Width, display_canvas.Image.Height, PixelFormat.Format32bppArgb);
            
            using (Graphics gr = Graphics.FromImage(final))
            {
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                
                gr.DrawImage(m_canvasImage,
                    new Rectangle(0, 0, m_canvasImage.Width, m_canvasImage.Height),
                    new Rectangle(0, 0, m_canvasImage.Width, m_canvasImage.Height),
                    GraphicsUnit.Pixel);
                gr.DrawImage(m_drawBuffer,
                    m_redrawDrawBufferZone,
                    m_redrawDrawBufferZone,
                    GraphicsUnit.Pixel);
                gr.Save();
            }
            
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();
            
            display_canvas.Image = final;
            drawUI();
            display_canvas.Refresh();
            
            m_mousePosPrev = mousePos;
        }
        
        /// <summary>
        /// Runs when mouse up event trggers.
        /// Sets m_isDrawing to false.
        /// Flattens all layers and buffer to m_finalImage.
        /// Copies Image to m_canvasImage and display_canvas.
        /// Clears and m_drawBuffer and resets m_redrawDrawBufferZone.
        /// Runs garbage collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void display_canvas_MouseUp(object sender, MouseEventArgs e)
        {
            m_isDrawing = false;
            flattenImage();

            if (m_canvasImage != null)
                m_canvasImage.Dispose();
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();

            m_canvasImage = (Bitmap)m_finalImage.Clone();
            display_canvas.Image = (Bitmap)m_canvasImage.Clone();

            drawUI();
            clearDrawBuffer();

            display_canvas.Refresh();
        }
        
        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        //              END ORGANIZED FUNCTIONS
        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Combines every layer and drawBuffer into a single image.
        /// </summary>
        private void flattenImage()
        {
            if (m_finalImage != null)
                m_finalImage.Dispose();
            m_finalImage = new Bitmap(display_canvas.Width, display_canvas.Height, PixelFormat.Format32bppArgb);

            // move pixels from drawBuffer to display_canvas
            // flatten all layers
            for (int l = 0; l < m_layerCount; l++)
            {
                Console.Write("\nLayer " + l + " isVisible = " + m_layers[l].getIsVisible());
                if (m_layers[l].getId() == int.MinValue || m_layers[l].getIsVisible() == false)
                    continue;
                
                Bitmap layer = m_layers[l].getImage();
                
                // apply drawbuffer if l is current layer
                if (l == m_layerCurrent)
                {
                    Console.Write("\nDrawing on layer " + l);
                    
                    Bitmap buffer = new Bitmap(layer.Width, layer.Height, PixelFormat.Format32bppArgb);

                    // cut mask section out of drawBuffer
                    using (Graphics m = Graphics.FromImage(buffer))
                    {
                        m.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        m.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                        m.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        m.DrawImage(m_drawBuffer,
                            new Rectangle(0, 0, buffer.Width, buffer.Height));
                    }

                    // add draw buffer to current layer
                    Bitmap bmp = mergeImages(layer, buffer);

                    m_layers[l].setImage(bmp);
                    buffer.Dispose();

                    // get a handle on the image again
                    layer = m_layers[l].getImage();
                } // END (l == m_layerCurrent)

                Bitmap final = mergeImages(m_finalImage, layer, BlendMode.DrawAdd);
                m_finalImage = final;
            } // END l
        }

        /// <summary>
        /// Draws the UI.
        /// Currently Draws a box along the edge of m_redrawDrawBufferZone to display_canvas.Image.
        /// </summary>
        private void drawUI()
        {
            // ouline drawbuffer redraw zone
            for (int i = m_redrawDrawBufferZone.X; i < m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width; i++)
            {
                int x = i;
                int bottom = m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height - 1;
                int top = m_redrawDrawBufferZone.Y;

                // skip pixels that are out of bounds
                if (x < 0 || x >= display_canvas.Width)
                    continue;
                if (bottom >= display_canvas.Height || bottom < 0) { }
                else
                    ((Bitmap)display_canvas.Image).SetPixel(x, bottom, Color.Black);
                if (top < 0 || top >= display_canvas.Height) { }
                else
                    ((Bitmap)display_canvas.Image).SetPixel(x, top, Color.Black);
            }
            for (int i = m_redrawDrawBufferZone.Y; i < m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height; i++)
            {
                int right = m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width - 1;
                int left = m_redrawDrawBufferZone.X;
                int y = i;

                // skip pixels that are out of bounds
                if (y < 0 || y >= display_canvas.Height)
                    continue;

                if (right >= display_canvas.Width || right < 0) { }
                else
                    ((Bitmap)display_canvas.Image).SetPixel(right, y, Color.Black);
                if (left < 0 || left >= display_canvas.Width) { }
                else
                    ((Bitmap)display_canvas.Image).SetPixel(left, y, Color.Black);
                
            }
            // END outline drawbuffer redraw zone
        }

        /// <summary>
        /// Draws to m_drawBuffer size of brush centered on (mousePosition)
        /// Expands m_redrawDrawBufferZone as needed
        /// </summary>
        /// <param name="mousePosition">The current position of the mouse within draw_canvas</param>
        private void drawToBuffer(Point mousePosition)
        {
            int brushHalfWidth = m_brush.Width / 2;
            int brushHalfHeight = m_brush.Height / 2;

            expandUpdateArea(mousePosition);

            // test drawing and brush orientation
            for (int y = 0; y < m_brush.Size.Height; y++)
                for (int x = 0; x < m_brush.Size.Width; x++)
                {
                    Point current = new Point(
                        mousePosition.X + x - brushHalfWidth, 
                        mousePosition.Y + y - brushHalfHeight
                        );

                    if (current.X < 0 || current.Y < 0)
                        continue;
                    if (current.X >= m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width || 
                        current.Y >= m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height)
                        continue;
                    // check drawbuffer bounds (drawbuffer size is limited to canvas saze atm)
                    if (current.X >= display_canvas.Image.Size.Width || current.Y >= display_canvas.Image.Size.Height)
                        continue;

                    Color bp = m_brush.GetPixel(x, y);
                    Color ip = m_drawBuffer.GetPixel(current.X, current.Y);

                    // alpha multiplier from brush
                    float bpAlphacoef = bp.A / (float)byte.MaxValue;

                    // colors after brush is considered
                    int dbA = (int)((bp.A / (float)byte.MaxValue) * m_color.A);
                    int dbR = (int)((bp.R / (float)byte.MaxValue) * m_color.R);
                    int dbG = (int)((bp.G / (float)byte.MaxValue) * m_color.G);
                    int dbB = (int)((bp.B / (float)byte.MaxValue) * m_color.B);

                    // preset final blended pixel values with previous pixel color
                    int a = ip.A;
                    int r = ip.R;
                    int g = ip.G;
                    int b = ip.B;
                    
                    switch (m_brushMode)
                    {
                        case BlendMode.DrawAdd:
                            {
                                if (bp.A == 0)
                                    break;

                                if (dbA > a)
                                    a = dbA;

                                r = dbR;
                                g = dbG;
                                b = dbB;
                                break;
                            }
                        case BlendMode.DrawMix:
                            { // mix
                                if (bp.A == 0)
                                    break;

                                float bpAlphaWeight = bp.A / (float)(ip.A + bp.A);
                                float ipAlphaWeight = 1.0f - bpAlphaWeight; // float ipAlphaWeight = ip.A / (float)(ip.A + bp.A);

                                a = ip.A + (int)((byte.MaxValue - ip.A) * bpAlphaWeight * bpAlphacoef);
                                r = (int)(ip.R * ipAlphaWeight + dbR * bpAlphaWeight);
                                g = (int)(ip.G * ipAlphaWeight + dbG * bpAlphaWeight);
                                b = (int)(ip.B * ipAlphaWeight + dbB * bpAlphaWeight);
                                break;
                            }
                        case BlendMode.Erase:
                            {
                                if(dbA > a)
                                    a = dbA;

                                r = byte.MaxValue;
                                g = byte.MaxValue;
                                b = byte.MaxValue;

                                break;
                            }
                        default: break;
                    } // END switch(m_brushMode)

                    // set blended pixel to image
                    m_drawBuffer.SetPixel(current.X, current.Y, Color.FromArgb(a, r, g, b));
                }
        }

        /// <summary>
        /// Checks the edge positions of m_redrawDrawBuffer.
        /// If (mousePos) is at the edge of m_redrawDrawBufferZone:
        ///    resize m_redrawDrawBufferZone based on direction and brush size.
        /// </summary>
        /// <param name="mousePos">The current position of the mouse within draw_canvas</param>
        /// <returns></returns>
        private bool expandUpdateArea(Point mousePos)
        {
            bool result = false;

            // number of pixels the redraw area expands by
            int increase = (display_canvas.Width + display_canvas.Height) / 16;

            Point moveDelta = new Point(
                mousePos.X - m_mousePosPrev.X,
                mousePos.Y - m_mousePosPrev.Y);

            int halfBrushWidth = m_brush.Size.Width;
            int halfBrushHeight = m_brush.Size.Height;

            if (moveDelta.X != 0)
            {
                int rightEdgePosition = m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width - halfBrushWidth;
                if (mousePos.X > rightEdgePosition)
                {
                    // expand right
                    m_redrawDrawBufferZone.Width += increase;
                }
                else if (mousePos.X < m_redrawDrawBufferZone.X + halfBrushWidth)
                {
                    // expand left
                    m_redrawDrawBufferZone.X -= increase;
                    m_redrawDrawBufferZone.Width += increase;
                }
                result = true;
            }

            if (moveDelta.Y != 0)
            {
                int bottomEdgePosition = m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height - halfBrushHeight;
                if (mousePos.Y > bottomEdgePosition)
                {
                    // expand down
                    m_redrawDrawBufferZone.Height += increase;
                }
                else if (mousePos.Y < m_redrawDrawBufferZone.Y + halfBrushHeight)
                {
                    // expand up
                    m_redrawDrawBufferZone.Y -= increase;
                    m_redrawDrawBufferZone.Height += increase;
                }
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Sets all pixels within m_redrawDrawBuffer to Color(0, 0, 0, 0).
        /// Sets all m_redrawDrawBuffer values to 0.
        /// </summary>
        private void clearDrawBuffer()
        {
            Point start = new Point(m_redrawDrawBufferZone.X, m_redrawDrawBufferZone.Y);
            Point end = new Point(start.X + m_redrawDrawBufferZone.Width, start.Y + m_redrawDrawBufferZone.Height);
            for (int i = start.Y; i < end.Y; i++)
                for (int j = start.X; j < end.X; j++)
                {
                    // bounds check
                    if (i < 0 || j < 0)
                        continue;
                    if (i >= display_canvas.Height || j >= display_canvas.Width)
                        continue;
                    m_drawBuffer.SetPixel(j, i, Color.FromArgb(0, 0, 0, 0));
                }
            m_redrawDrawBufferZone.X =
                m_redrawDrawBufferZone.Y =
                m_redrawDrawBufferZone.Width =
                m_redrawDrawBufferZone.Height = 0;
        }

        private void deleteLayer(int position)
        {
            if (m_layerCount <= 0)
                return;

            // shift following layers to the left
            int end = m_layerCount - 1;
            for (int i = position; i < end; i++)
            {
                m_layers[i] = m_layers[i + 1];
                m_layers[i].setId(i);
            }

            // clear last layer
            m_layers[m_layerCount].clearLayer();
            m_layerCount--;
        }

        /// <summary>
        /// Merges two images together based on brush mode. (source) and (overlay) must be the same length.
        /// </summary>
        /// <param name="source">The image used as a base.</param>
        /// <param name="overlay">The image applied to the base</param>
        /// <param name="mode">The method used to blend images together</param>
        /// <returns></returns>
        private Bitmap mergeImages(Bitmap source, Bitmap overlay, BlendMode mode = BlendMode.None)
        {
            BlendMode bm = m_brushMode;
            if (mode != BlendMode.None)
                bm = mode;

            // size in bytes of pixels
            int pixelSize = 4;
            
            // prepare source data for modification
            Bitmap sourceBmp = (Bitmap)source.Clone();
            PixelFormat sourcePxf = PixelFormat.Format32bppArgb;
            Rectangle sourceRect = new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height);
            BitmapData sourceBmpData = sourceBmp.LockBits(sourceRect, ImageLockMode.ReadWrite, sourcePxf);

            // prepare overlay data for modification
            Bitmap overlayBmp = (Bitmap)overlay.Clone();
            PixelFormat overlayPxf = PixelFormat.Format32bppArgb;
            Rectangle overlayRect = new Rectangle(0, 0, overlayBmp.Width, overlayBmp.Height);
            BitmapData overlayBmpData = overlayBmp.LockBits(overlayRect, ImageLockMode.ReadWrite, overlayPxf);
            
            IntPtr sourcePtr = sourceBmpData.Scan0;
            IntPtr overlayPtr = overlayBmpData.Scan0;

            int sourceNumBytes = sourceBmp.Width * sourceBmp.Height * pixelSize;
            byte[] sourceArgbValues = new byte[sourceNumBytes];

            int overlayNumBytes = overlayBmp.Width * overlayBmp.Height * pixelSize;
            byte[] overlayArgbValues = new byte[overlayNumBytes];

            System.Diagnostics.Debug.Assert(sourceArgbValues.Length == overlayArgbValues.Length, "\nMerged images are not the same size.");

            System.Runtime.InteropServices.Marshal.Copy(sourcePtr, sourceArgbValues, 0, sourceNumBytes);
            System.Runtime.InteropServices.Marshal.Copy(overlayPtr, overlayArgbValues, 0, overlayNumBytes);
            
            for (int i = 0; i < sourceArgbValues.Length; i += pixelSize)
            {
                mergePixels(ref sourceArgbValues, ref overlayArgbValues, i, bm);
            }

            System.Runtime.InteropServices.Marshal.Copy(sourceArgbValues, 0, sourcePtr, sourceNumBytes);
            sourceBmp.UnlockBits(sourceBmpData);
            overlayBmp.UnlockBits(overlayBmpData);

            return sourceBmp;
        }

        /// <summary>
        /// Blends pixels together based on Brush Mode.
        /// </summary>
        /// <param name="source">The pixel array to be used as a base.</param>
        /// <param name="overlay">The pixel array to be applied to the base.</param>
        /// <param name ="offset">The position of the target pixel within the array</param>
        /// <returns></returns>
        private void mergePixels(ref byte[] source, ref byte[] overlay, int offset, BlendMode mode)
        {
            // argbValues are in format BGRA (Blue, Green, Red, Alpha)
            int b = offset;
            int g = offset + 1;
            int r = offset + 2;
            int a = offset + 3;
            switch (mode)
            {
                case BlendMode.DrawAdd:
                    {
                        // skip if overlay has 0% opacity
                        if (overlay[a] == 0)
                            break;
                        
                        float overAcoef = overlay[a] / (float)byte.MaxValue;
                        float sourAcoef = source[a] / (float)byte.MaxValue;
                        
                        float A = 1.0f - (1.0f - overAcoef) * (1.0f - sourAcoef);
                        source[a] = (byte)(byte.MaxValue * A);
                        
                        if (A < 1.0e-6)
                            break;
                        
                        source[r] = (byte)(overlay[r] * overAcoef / A + source[r] * sourAcoef * (1.0f - overAcoef) / A);
                        source[g] = (byte)(overlay[g] * overAcoef / A + source[g] * sourAcoef * (1.0f - overAcoef) / A);
                        source[b] = (byte)(overlay[b] * overAcoef / A + source[b] * sourAcoef * (1.0f - overAcoef) / A);
                        
                        break;
                    } // END case BlendMode.DrawAdd
                case BlendMode.DrawMix:
                    {
                        // skip if overlay has 0% opacity
                        if (overlay[a] == 0)
                            break;
                        
                        float overAcoef = overlay[a] / (float)byte.MaxValue;
                        float sourAweight = source[a] / (float)(source[a] + overlay[a]);
                        float overAweight = 1.0f - sourAweight;
                        
                        source[a] = (byte)((overlay[a] * overAweight + source[a] * sourAweight));
                        source[r] = (byte)((overlay[r] * overAweight + source[r] * sourAweight));
                        source[g] = (byte)((overlay[g] * overAweight + source[g] * sourAweight));
                        source[b] = (byte)((overlay[b] * overAweight + source[b] * sourAweight));

                        break;
                    } // END case BlendMode.DrawMix
                case BlendMode.Replace:
                    {
                        source[a] = overlay[a];
                        source[r] = overlay[r];
                        source[g] = overlay[g];
                        source[b] = overlay[b];
                        break;
                    }
                case BlendMode.Erase:
                    {
                        // skip if overlay has 0% opacity
                        if (overlay[a] == 0)
                            break;
                        if (overlay[a] >= source[a])
                            source[a] = 0;
                        else
                            source[a] = (byte)(source[a] - overlay[a]);

                        // If 0% transparent change colors to white
                        if (source[a] > 0)
                            break;

                        // turn 0% opacity pixels white
                        source[r] = byte.MaxValue;
                        source[g] = byte.MaxValue;
                        source[b] = byte.MaxValue;
                        break;
                    } // END case BlendMode.Erase
                case BlendMode.None: break;
                default: Console.Write("\nUnknown Blend Mode"); break;
            } // END switch(br)
        }

        private void doesNothing() { /*   ONLY USED SO THAT GENERATED CODE APPEARS BELOW HERE    */}

        private void button_square_brush_Click(object sender, EventArgs e)
        {
            if (m_brush != null)
                m_brush.Dispose();

            button_round_brush.BackColor = Color.Transparent;
            button_square_brush.BackColor = Color.Black;

            m_brush = new Bitmap(30, 30);
            for (int i = 0; i < m_brush.Size.Height; i++)
                for (int j = 0; j < m_brush.Size.Width; j++)
                {
                    m_brush.SetPixel(j, i, Color.White);
                }
        }

        private void button_round_brush_Click(object sender, EventArgs e)
        {
            if (m_brush != null)
                m_brush.Dispose();

            button_round_brush.BackColor = Color.Black;
            button_square_brush.BackColor = Color.Transparent;

            string dir = Environment.CurrentDirectory + m_defaultBrush;
            if (System.IO.File.Exists(dir))
            {
                m_brush = new Bitmap(dir);
                for (int i = 0; i < m_brush.Size.Height; i++)
                    for (int j = 0; j < m_brush.Size.Width; j++)
                    {
                        Color c = m_brush.GetPixel(j, i);
                        Color inverted = Color.FromArgb(
                            c.A,
                            byte.MaxValue - c.R,
                            byte.MaxValue - c.G,
                            byte.MaxValue - c.B);
                        m_brush.SetPixel(j, i, inverted);
                    }
            }
            else
            {
                Console.Write("\nDefault brush not found: " + dir + "\nCreating new brush");
                Bitmap img = new Bitmap(30, 30, PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(img))
                {
                    gr.Clear(Color.Black);
                }
                m_brush = img;
                for (int i = 0; i < m_brush.Size.Height; i++)
                    for (int j = 0; j < m_brush.Size.Width; j++)
                    {
                        Color c = m_brush.GetPixel(j, i);
                        if ((j + i) % 2 == 1)
                            m_brush.SetPixel(j, i, Color.Transparent);
                    }
            }
        }

        private void layer_1_Click(object sender, EventArgs e)
        {
            m_layerCurrent = 0;
            layer_1.BackColor = Color.Black;
            layer_2.BackColor = Color.Transparent;
            layer_3.BackColor = Color.Transparent;
        }

        private void layer_2_Click(object sender, EventArgs e)
        {
            m_layerCurrent = 1;
            layer_1.BackColor = Color.Transparent;
            layer_2.BackColor = Color.Black;
            layer_3.BackColor = Color.Transparent;
        }

        private void layer_3_Click(object sender, EventArgs e)
        {
            m_layerCurrent = 2;
            layer_1.BackColor = Color.Transparent;
            layer_2.BackColor = Color.Transparent;
            layer_3.BackColor = Color.Black;
        }

        private void layer_1_visible_Click(object sender, EventArgs e)
        {
            bool isVisible = m_layers[0].getIsVisible();
            isVisible = !isVisible;
            m_layers[0].setIsVisible(isVisible);

            if (isVisible)
                layer_1_visible.BackColor = Color.Black;
            else
                layer_1_visible.BackColor = Color.Transparent;

            flattenImage();
            if (m_canvasImage != null)
                m_canvasImage.Dispose();
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();
            m_canvasImage = new Bitmap(m_finalImage);
            display_canvas.Image = (Bitmap)m_canvasImage.Clone();
            display_canvas.Refresh();
        }

        private void layer_2_visible_Click(object sender, EventArgs e)
        {
            bool isVisible = m_layers[1].getIsVisible();
            isVisible = !isVisible;
            m_layers[1].setIsVisible(isVisible);

            if (isVisible)
                layer_2_visible.BackColor = Color.Black;
            else
                layer_2_visible.BackColor = Color.Transparent;

            flattenImage();
            if (m_canvasImage != null)
                m_canvasImage.Dispose();
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();
            m_canvasImage = new Bitmap(m_finalImage);
            display_canvas.Image = (Bitmap)m_canvasImage.Clone();
            display_canvas.Refresh();
        }

        private void layer_3_visible_Click(object sender, EventArgs e)
        {
            bool isVisible = m_layers[2].getIsVisible();
            isVisible = !isVisible;
            m_layers[2].setIsVisible(isVisible);

            if (isVisible)
                layer_3_visible.BackColor = Color.Black;
            else
                layer_3_visible.BackColor = Color.Transparent;

            flattenImage();
            if (m_canvasImage != null)
                m_canvasImage.Dispose();
            if (display_canvas.Image != null)
                display_canvas.Image.Dispose();
            m_canvasImage = new Bitmap(m_finalImage);
            display_canvas.Image = (Bitmap)m_canvasImage.Clone();
            display_canvas.Refresh();
        }
    }
}
