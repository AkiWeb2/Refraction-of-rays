using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox;
        private Label label;
        private TextBox textBox;
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private iTextSharp.text.Font helvetica;
        private BaseFont helveticaBase;

        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            ClientSize = new Size(800, 310);

            pictureBox = new PictureBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(250, 200),
                Location = new Point(100, 25),
            };
            label = new Label
            {
                Text = "Введите угол падения (в градусах):",
                Size = new Size(200, 20),
                Location = new Point(100, 230),
            };

            textBox = new TextBox
            {
                Size = new Size(50, 20),
                Location = new Point(110, 250),
            };
            Controls.Add(pictureBox);
            Controls.Add(label);
            Controls.Add(textBox);
            textBox.TextChanged += UpdatePictureBox;
        }

        private void UpdatePictureBox(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            float angle;
            bool parseSuccess = float.TryParse(textBox.Text, out angle);
            if (angle > 0)
            {
                if (parseSuccess)
                {
                    // Вычислить показатель преломления среды и угол преломления
                    float refractiveIndex = 1.5f; // Здесь необходимо указать фактический показатель преломления среды
                    float WaterIndex = 1.33f;
                    float AlmIndex = 2.42f;
                    float angleOfReWater = (float)(Math.Asin(Math.Sin(angle * Math.PI / 180) / WaterIndex) * 180 / Math.PI);
                    float angleOfReAlm = (float)(Math.Asin(Math.Sin(angle * Math.PI / 180) / AlmIndex) * 180 / Math.PI);
                    float angleOfRefraction = (float)(Math.Asin(Math.Sin(angle * Math.PI / 180) / refractiveIndex) * 180 / Math.PI);
                    label2.Text = angleOfRefraction.ToString();

                    // Очистить PictureBox и нарисовать лучи
                    using (var graphics = pictureBox.CreateGraphics())
                    {
                        graphics.Clear(Color.White);
                        Pen normalPen = new Pen(Color.Black, 2);
                        Pen normalPenP = new Pen(Color.Black, 2);
                        normalPenP.DashStyle = DashStyle.Dash;
                        graphics.DrawLine(normalPenP, new Point(0, 50), new Point(275, 50));
                        graphics.DrawLine(normalPen, new Point(133, 0), new Point(133, 200));

                        Pen incidentRayPen = new Pen(Color.Red, 2);
                        Pen incidentRay = new Pen(Color.Orange, 2);
                        if (angle > 180)
                        {
                            incidentRay.DashStyle = DashStyle.Dash;
                            graphics.DrawLine(incidentRayPen, new Point(20, (int)angle + 40), new Point(130, 50));
                            graphics.DrawLine(incidentRay, new Point(20, (int)angle - 40), new Point(130, 50));
                        }
                        else if (angle > 90 && angle != 180)
                        {
                            incidentRay.DashStyle = DashStyle.Dash;
                            graphics.DrawLine(incidentRayPen, new Point(130, 50), new Point(285, (int)angle + 60));
                            graphics.DrawLine(incidentRay, new Point(130, 50), new Point(285, (int)angle - 60));
                        }
                        else if (angle <= 90)
                        {
                            incidentRay.DashStyle = DashStyle.Dash;
                            graphics.DrawLine(incidentRayPen, new Point(130, 50), new Point(285, (int)angle - 40));
                            graphics.DrawLine(incidentRay, new Point(130, 50), new Point(285, (int)angle + 40));
                        }
                        else if (angle == 180)
                        {
                            incidentRay.DashStyle = DashStyle.Dash;
                            graphics.DrawLine(incidentRayPen, new Point(133, 50), new Point(133, 200));
                            graphics.DrawLine(incidentRay, new Point(133, 50), new Point(285, -(int)angle + 40));
                        }


                        Pen refractedRayPen = new Pen(Color.Blue, 2);
                        if (checkBox2.Checked == true)
                        {
                            double Px = angleOfRefraction + 100 * Math.Cos(angle);
                            //double Py = angleOfRefraction + 100 * Math.Sin(angle);
                            float arrowAngle = (float)(Math.Atan2(25, 50 - 125) * 180 / Math.PI);
                            if (angle < 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(135, 50), new Point((int)-arrowAngle, (int)-Px - 40));
                            }
                            else if (angle > 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(133, 50), new Point((int)arrowAngle, (int)Px + 40));
                            }
                            else if (angle == 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(135, 50), new Point((int)-arrowAngle, (int)Px));
                            }
                        }
                        else if (checkBox1.Checked == true)
                        {
                            double Px = angleOfReWater + 100 * Math.Cos(angle);
                            //double Py = angleOfRefraction + 100 * Math.Sin(angle);
                            float arrowAngle = (float)(Math.Atan2(25, 50 - 125) * 180 / Math.PI);
                            if (angle < 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(135, 50), new Point((int)-arrowAngle, (int)-Px - 40));
                            }
                            else if (angle > 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(133, 50), new Point((int)arrowAngle, (int)Px + 40));
                            }
                            else if (angle == 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(135, 50), new Point((int)-arrowAngle, (int)Px));
                            }
                        }
                        else if (checkBox3.Checked == true)
                        {
                            double Px = AlmIndex + 100 * Math.Cos(angle);
                            //double Py = angleOfRefraction + 100 * Math.Sin(angle);
                            float arrowAngle = (float)(Math.Atan2(25, 50 - 125) * 180 / Math.PI);
                            if (angle < 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(135, 50), new Point((int)-arrowAngle, (int)-Px - 40));
                            }
                            else if (angle > 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(133, 50), new Point((int)arrowAngle, (int)Px + 40));
                            }
                            else if (angle == 180)
                            {
                                graphics.DrawLine(refractedRayPen, new Point(135, 50), new Point((int)-arrowAngle, (int)Px));
                            }
                        }
                        graphics.DrawLine(refractedRayPen, new Point(275, 100), new Point(275 - 10, 100 - 5));
                        graphics.DrawLine(refractedRayPen, new Point(275, 100), new Point(275 - 10, 100 + 5));
                        if (angle <= 180)
                        {
                            graphics.DrawString("θ1", Font, Brushes.Red, new PointF(150, 35));
                            graphics.DrawString("θ2", Font, Brushes.Blue, new PointF(55, 60));
                        }
                        else if (angle > 180)
                        {
                            graphics.DrawString("θ1", Font, Brushes.Red, new PointF(55, 60));
                            graphics.DrawString("θ2", Font, Brushes.Blue, new PointF(150, 35));
                        }


                        float angleOfIncidenceLabelX = (225 + 125) / 2 - 10;
                        float angleOfRefractionLabelX = (25 + 75) / 2 - 10;
                        if (checkBox2.Checked == true)
                        {
                            if (angle <= 180)
                            {
                                graphics.DrawString(angle.ToString(), Font, Brushes.Black, new PointF(angleOfIncidenceLabelX, 55));
                                graphics.DrawString(angleOfRefraction.ToString(), Font, Brushes.Black, new PointF(angleOfRefractionLabelX, 35));
                            }
                            else if (angle > 180)
                            {
                                graphics.DrawString(angle.ToString(), Font, Brushes.Black, new PointF(angleOfRefractionLabelX, 50));
                                graphics.DrawString(angleOfRefraction.ToString(), Font, Brushes.Black, new PointF(angleOfIncidenceLabelX, 40));
                            }
                        }
                        else if (checkBox1.Checked == true)
                        {
                            if (angle <= 180)
                            {
                                graphics.DrawString(angle.ToString(), Font, Brushes.Black, new PointF(angleOfIncidenceLabelX, 55));
                                graphics.DrawString(angleOfReWater.ToString(), Font, Brushes.Black, new PointF(angleOfRefractionLabelX, 35));
                            }
                            else if (angle > 180)
                            {
                                graphics.DrawString(angle.ToString(), Font, Brushes.Black, new PointF(angleOfRefractionLabelX, 50));
                                graphics.DrawString(angleOfReWater.ToString(), Font, Brushes.Black, new PointF(angleOfIncidenceLabelX, 40));
                            }
                        }
                        else if (checkBox3.Checked == true)
                        {
                            if (angle <= 180)
                            {
                                graphics.DrawString(angle.ToString(), Font, Brushes.Black, new PointF(angleOfIncidenceLabelX, 55));
                                graphics.DrawString(angleOfReAlm.ToString(), Font, Brushes.Black, new PointF(angleOfRefractionLabelX, 35));
                            }
                            else if (angle > 180)
                            {
                                graphics.DrawString(angle.ToString(), Font, Brushes.Black, new PointF(angleOfRefractionLabelX, 50));
                                graphics.DrawString(angleOfReAlm.ToString(), Font, Brushes.Black, new PointF(angleOfIncidenceLabelX, 40));
                            }
                        }
                    }
                }
            }else if (angle < 0)
            {
                while (angle < 0)
                {
                    MessageBox.Show("Угол не может быть меньше 0");
                    break;
                }  
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.AllowTransparency = true;
            this.BackColor = Color.AliceBlue;//цвет фона  
            //this.TransparencyKey = this.BackColor;//он же будет заменен на прозрачный цвет
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var document = new iTextSharp.text.Document();
            using (var writer = PdfWriter.GetInstance(document, new FileStream("result.pdf", FileMode.Create)))
            {
                document.Open();

                helvetica = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);
                helveticaBase = helvetica.GetCalculatedBaseFont(false);
                PdfPTable table = new PdfPTable(3);
                PdfPCell cell = new PdfPCell(new Phrase(" ",
                 new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16,
                 iTextSharp.text.Font.NORMAL, new BaseColor(Color.Black))));
                cell.BackgroundColor = new BaseColor(Color.White);
                cell.Padding = 1;
                cell.Colspan = 1;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                table.AddCell(textBox.Text);
                table.AddCell(label2.Text);
                table.AddCell(cell);
                document.Add(table);
                document.Close();
                writer.Close();
            }
        }
    }
}
