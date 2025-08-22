namespace MandelbrotsApple
{
    partial class MandelbrotForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private DoubleBufferedPanel canvasPanel;
        private System.Windows.Forms.TrackBar sliderIteration;
        private System.Windows.Forms.Label labelIterations;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "MandelbrotsApple";

            this.components = new System.ComponentModel.Container();
            this.canvasPanel = new DoubleBufferedPanel();
            this.sliderIteration = new System.Windows.Forms.TrackBar();
            this.labelIterations = new System.Windows.Forms.Label();

            // 
            // canvasPanel
            // 
            this.canvasPanel.Location = new System.Drawing.Point(20, 20);
            this.canvasPanel.Size = new System.Drawing.Size(760, 350); // Erhöht auf 350
            this.canvasPanel.BackColor = System.Drawing.Color.White;
            this.canvasPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvasPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            // 
            // labelIterations
            // 
            this.labelIterations.Location = new System.Drawing.Point(20, 380); // 20 + 350 + 10
            this.labelIterations.Size = new System.Drawing.Size(80, 20);
            this.labelIterations.Text = "Iterations";
            this.labelIterations.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;

            // 
            // sliderIteration
            // 
            this.sliderIteration.Location = new System.Drawing.Point(110, 380); // 20 + 350 + 10
            this.sliderIteration.Size = new System.Drawing.Size(670, 30);
            this.sliderIteration.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sliderIteration.Scroll += new System.EventHandler(this.On_SliderIteration_Scroll);
            this.sliderIteration.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.sliderIteration.Minimum = 0;
            this.sliderIteration.Maximum = 100;
            this.sliderIteration.Value = 0;

            // 
            // Form1
            // 
            this.Controls.Add(this.canvasPanel);
            this.Controls.Add(this.labelIterations);
            this.Controls.Add(this.sliderIteration);
            this.Text = "Form1";
        }
        #endregion
    }
}
