using System.Diagnostics;
using Talej_Image_Processor_CMSC_630.Properties;

namespace Talej_Image_Processor_CMSC_630
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            inputTextBox.Text = Settings.Default.InputPath;
            outputTextBox.Text = Settings.Default.OutputPath;
            inputTextBox.ReadOnly = true;
            outputTextBox.ReadOnly = true;
            comboBox1.Items.Insert(0, "None");
            comboBox1.Items.Insert(1, "Linear 3x3 (Sharpen Preset)");
            comboBox1.Items.Insert(2, "Linear 5x5 (Sharpen Preset)");
            comboBox1.Items.Insert(3, "Median 3x3");
            comboBox1.Items.Insert(4, "Median 5x5");
            comboBox1.Items.Insert(5, "Linear 5x5 (Gaussian Blur Preset)");
            filterBiasTextBox.Text = "0.0";
            filterFactorTextBox.Text = "1.0";
            comboBox1.SelectedIndex = 0;
            quantLevelSlider.Value = 8;
            Matrix3x3.Visible = false;
            Matrix5x5.Visible = false;
            if (this.Original.Image == null || Settings.Default.ColorChoice == null)
            {
                button2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openImage  = new OpenFileDialog();
            openImage.Filter = "Image File (*.bmp) | *.bmp";

            if (DialogResult.OK == openImage.ShowDialog())
            {
                this.Original.Image = ImageProcessing.ResizeImage(new Bitmap(openImage.FileName),288,219);
                button2.Enabled = true;
                Settings.Default.SelectedImage = openImage.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int filterSelection = comboBox1.SelectedIndex;
            double factor = Double.Parse(filterFactorTextBox.Text);
            double bias = Double.Parse(filterBiasTextBox.Text);
            int impulseInput = impulsenoiseintensityslider.Value;
            int gaussianInput = gaussianNoiseSlider.Value;
            int quantInput = quantLevelSlider.Value;

            List<double> MaskWeights3x3 = new List<double>();
            List<double> MaskWeights5x5 = new List<double>();
            if (filterSelection == 1 || filterSelection == 3)
            {
                MaskWeights3x3.Add(double.Parse(M3x300.Text));
                MaskWeights3x3.Add(double.Parse(M3x301.Text));
                MaskWeights3x3.Add(double.Parse(M3x302.Text));
                MaskWeights3x3.Add(double.Parse(M3x310.Text));
                MaskWeights3x3.Add(double.Parse(M3x311.Text));
                MaskWeights3x3.Add(double.Parse(M3x312.Text));
                MaskWeights3x3.Add(double.Parse(M3x320.Text));
                MaskWeights3x3.Add(double.Parse(M3x321.Text));
                MaskWeights3x3.Add(double.Parse(M3x322.Text));
            }

            if (filterSelection == 2 || filterSelection == 4 || filterSelection == 5)
            {
                MaskWeights5x5.Add(double.Parse(Matrix5x500.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x501.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x502.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x503.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x504.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x510.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x511.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x512.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x513.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x514.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x520.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x521.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x522.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x523.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x524.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x530.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x531.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x532.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x533.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x534.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x540.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x541.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x542.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x543.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x544.Text));
            }



            var grayscaletimer = new Stopwatch();
            var impulseNoiseTimer = new Stopwatch();
            var gaussianNoiseTimer = new Stopwatch();  
            var histGenTimer = new Stopwatch();
            var histEqImageTimer = new Stopwatch();
            var quantizationTimer = new Stopwatch();
            var linearTimer = new Stopwatch();
            var medianFilterTimer = new Stopwatch();

            Bitmap copy = new Bitmap(Settings.Default.SelectedImage);

            grayscaletimer.Start();
            Bitmap modifiedImage = ImageProcessing.RGB2GrayscaleImage(copy, Settings.Default.ColorChoice);
            //byte[] grayscaleImageBuffer = ImageProcessing.RGB2GrayscaleImage(copy, Settings.Default.ColorChoice);
            grayscaletimer.Stop();
            //Bitmap grayscaleImage = ImageProcessing.CreateImage(grayscaleImageBuffer, copy.Width, copy.Height);
            //this.Modified.Image = ImageProcessing.ResizeImage(grayscaleImage, 288, 219);
            this.Modified.Image = ImageProcessing.ResizeImage(modifiedImage, 288, 219);


            histEqImageTimer.Start();
            //Bitmap eqImg = ImageProcessing.HistogramEqualizedImage(modifiedImage);
            Bitmap eqImg = ImageProcessing.HistogramEqualizedImage(modifiedImage);
            histEqImageTimer.Stop();

            if (impulseInput > 0)
            {
                impulseNoiseTimer.Start();
                Bitmap ImpulseNoiseImage = ImageProcessing.ImpulseNoise(modifiedImage, impulseInput);
                //Bitmap ImpulseNoiseImage = ImageProcessing.ImpulseNoise(grayscaleImageBuffer, impulsenoiseintensityslider.Value, copy.Width, copy.Height);
                impulseNoiseTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(ImpulseNoiseImage, 288, 219);
            }

            if (gaussianInput > 0)
            {
                gaussianNoiseTimer.Start();
                Bitmap GaussianNoiseImage = ImageProcessing.GaussianNoise(modifiedImage, gaussianInput);
                gaussianNoiseTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(GaussianNoiseImage, 288, 219);
            }


            if(filterSelection == 1)
            {
                linearTimer.Start();
                Bitmap linearImage = ImageProcessing.LinearFilter3x3(modifiedImage, MaskWeights3x3, factor, bias);
                linearTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(linearImage, 288, 219);
            }

            if (filterSelection == 2)
            {
                linearTimer.Start();
                //Bitmap linearImage5x5 = ImageProcessing.Linear5x5(modifiedImage, MaskWeights5x5, Double.Parse(filterFactorTextBox.Text), Double.Parse(filterBiasTextBox.Text));
                Bitmap linearImage5x5 = ImageProcessing.LinearFilter5x5(modifiedImage, MaskWeights5x5, factor, bias);
                linearTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(linearImage5x5, 288, 219);
            }
            if (filterSelection == 3)
            {
                medianFilterTimer.Start();
                Bitmap MedianFilteredImage = ImageProcessing.MedianFilter3x3(modifiedImage, MaskWeights3x3);
                medianFilterTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(MedianFilteredImage, 288, 219);
            }

            if (filterSelection == 4)
            {
                medianFilterTimer.Start();
                Bitmap MedianFilteredImage = ImageProcessing.MedianFilter5x5(modifiedImage, MaskWeights5x5);
                medianFilterTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(MedianFilteredImage, 288, 219);
            }

            if (filterSelection == 5)
            {
                linearTimer.Start();
                //Bitmap linearImage5x5 = ImageProcessing.Linear5x5(modifiedImage, MaskWeights5x5, Double.Parse(filterFactorTextBox.Text), Double.Parse(filterBiasTextBox.Text));
                Bitmap linearImage5x5 = ImageProcessing.LinearFilter5x5(modifiedImage, MaskWeights5x5, factor, bias);
                linearTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(linearImage5x5, 288, 219);
            }
            histGenTimer.Start();
            Bitmap histogramimage = ImageProcessing.GenerateHistogram(modifiedImage);
            histGenTimer.Stop();


            quantizationTimer.Start();
            Bitmap imgQuant = ImageProcessing.UniformImageQuantization(modifiedImage, quantInput, null);
            //Bitmap imgQuant = ImageProcessing.ImageQuantization(modifiedImage, quantLevelSlider.Value);
            quantizationTimer.Stop();
            this.histogramImage.Image = ImageProcessing.ResizeImage(histogramimage, 288, 219);
            this.histEqImage.Image = ImageProcessing.ResizeImage(eqImg, 288, 219);
            this.quantImage.Image = ImageProcessing.ResizeImage(imgQuant, 288, 219);

            #region Display Metrics
            singleGrayscaleTimeBox.Text = (grayscaletimer.ElapsedMilliseconds).ToString() + " ms";
            singleImpulseTimeBox.Text = (impulseNoiseTimer.ElapsedMilliseconds).ToString() + " ms";
            singleGaussianTimeBox.Text = (gaussianNoiseTimer.ElapsedMilliseconds).ToString() + " ms";
            singleHistogramTimeBox.Text = (histGenTimer.ElapsedMilliseconds).ToString() + " ms";
            singleHistEqTimeBox.Text = (histEqImageTimer.ElapsedMilliseconds).ToString() + " ms";
            singleQuantizationTimeBox.Text = (quantizationTimer.ElapsedMilliseconds).ToString() + " ms";
            quantErrorBox.Text = ImageProcessing.CalculateUniformImageQuantizationError(modifiedImage, quantInput).ToString();
            singleSharpenTimebox.Text = (linearTimer.ElapsedMilliseconds).ToString() + " ms";
            singleMedianFilterTimeBox.Text = (medianFilterTimer.ElapsedMilliseconds).ToString() + " ms";
            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    System.Windows.Forms.MessageBox.Show("Input Path Set. Files found: " + files.Length.ToString(), "Message");

                    Settings.Default["InputPath"] = fbd.SelectedPath;
                    Properties.Settings.Default.Save();
                    inputTextBox.Text = fbd.SelectedPath;

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Settings.Default["OutputPath"] = fbd.SelectedPath;
                    Properties.Settings.Default.Save();
                    outputTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int filterSelection = comboBox1.SelectedIndex;
            double factor = Double.Parse(filterFactorTextBox.Text);
            double bias = Double.Parse(filterBiasTextBox.Text);
            int impulseInput = impulsenoiseintensityslider.Value;
            int gaussianInput = gaussianNoiseSlider.Value;
            int quantInput = quantLevelSlider.Value;

            List<double> MaskWeights3x3 = new List<double>();
            List<double> MaskWeights5x5 = new List<double>();
            if (filterSelection == 1 || filterSelection == 3)
            {
                MaskWeights3x3.Add(double.Parse(M3x300.Text));
                MaskWeights3x3.Add(double.Parse(M3x301.Text));
                MaskWeights3x3.Add(double.Parse(M3x302.Text));
                MaskWeights3x3.Add(double.Parse(M3x310.Text));
                MaskWeights3x3.Add(double.Parse(M3x311.Text));
                MaskWeights3x3.Add(double.Parse(M3x312.Text));
                MaskWeights3x3.Add(double.Parse(M3x320.Text));
                MaskWeights3x3.Add(double.Parse(M3x321.Text));
                MaskWeights3x3.Add(double.Parse(M3x322.Text));
            }

            if (filterSelection == 2 || filterSelection == 4 || filterSelection == 5)
            {
                MaskWeights5x5.Add(double.Parse(Matrix5x500.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x501.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x502.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x503.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x504.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x510.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x511.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x512.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x513.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x514.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x520.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x521.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x522.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x523.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x524.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x530.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x531.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x532.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x533.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x534.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x540.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x541.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x542.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x543.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x544.Text));
            }
            var totaltimer = new Stopwatch();
            totaltimer.Start();
            string inputPath = Settings.Default["InputPath"].ToString();
            string outputPath = Settings.Default["OutputPath"].ToString();
            var files = new DirectoryInfo(inputPath)
            .GetFiles()
            .Where(f => f.IsImage());

            if(String.IsNullOrWhiteSpace(inputPath) && String.IsNullOrWhiteSpace(outputPath))
            {
                button5.Enabled = false;
            }
            var batchGrayscaletimer = new Stopwatch();
            var batchImpulseNoiseTimer = new Stopwatch();
            var batchGaussianNoiseTimer = new Stopwatch();
            var batchHistGenTimer = new Stopwatch();
            var batchHistEqImageTimer = new Stopwatch();
            var batchQuantizationTimer = new Stopwatch();
            var batchLinearTimer = new Stopwatch();
            var batchMedianTimer = new Stopwatch();

            //Original single threaded execution process
            foreach (var file in files)
            {
                using (Bitmap image = (Bitmap)Bitmap.FromFile(file.FullName))
                {
                    batchGrayscaletimer.Start();
                    using (var grayscaleImage = ImageProcessing.RGB2GrayscaleImage(image, Settings.Default.ColorChoice))
                    //using (var grayscaleImageBuffer = ImageProcessing.RGB2GrayscaleImage(image, Settings.Default.ColorChoice))
                    {
                        batchGrayscaletimer.Stop();
                        var grayscaleImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_grayscale_conversion" + file.Extension);
                        grayscaleImage.Save(grayscaleImageName);

                        //Begin Impulse Noise
                        if (impulseInput > 0)
                        {
                                batchImpulseNoiseTimer.Start();
                                var ImpulsedImage = ImageProcessing.ImpulseNoise(grayscaleImage, impulseInput);
                                batchImpulseNoiseTimer.Stop();
                                var newImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_processed_impulse_noise" + file.Extension);
                                ImpulsedImage.Save(newImageName);


                        }
                        //Begin Gaussian Noise 
                        if (gaussianInput > 0)
                        {

                                batchGaussianNoiseTimer.Start();
                                var GaussianNoiseImage = ImageProcessing.GaussianNoise(grayscaleImage, gaussianInput);
                                batchGaussianNoiseTimer.Stop();
                                var newImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_processed_gaussian_noise" + file.Extension);
                                GaussianNoiseImage.Save(newImageName);


                        }


                            //Begin histogram generation
                            batchHistGenTimer.Start();
                            var HistogramImage = ImageProcessing.GenerateHistogram(grayscaleImage);
                            batchHistGenTimer.Stop();
                            var histogramImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_histogram" + file.Extension);
                            HistogramImage.Save(histogramImageName);

                            //Begin histogram equalization
                            batchHistEqImageTimer.Start();
                            var HistogramEqImage = ImageProcessing.HistogramEqualizedImage(grayscaleImage);
                            batchHistEqImageTimer.Stop();

                            var histogramEqImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_histogram_equalized" + file.Extension);
                            HistogramEqImage.Save(histogramEqImageName);


                        //Begin Filtering
                        if (filterSelection == 1)
                        {

                                batchLinearTimer.Start();
                                var linearImage = ImageProcessing.LinearFilter3x3(grayscaleImage, MaskWeights3x3, factor, bias);

                                batchLinearTimer.Stop();
                                var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_3x3_sharpen" + file.Extension);
                                linearImage.Save(linearImageName);

 
                        }

                        if (filterSelection == 2)
                        {

                                batchLinearTimer.Start();
                                var linearImage = ImageProcessing.LinearFilter5x5(grayscaleImage, MaskWeights5x5, factor, bias);

                                batchLinearTimer.Stop();
                                var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_5x5_sharpen" + file.Extension);
                                linearImage.Save(linearImageName);

                            

                        }

                        if (filterSelection == 3)
                        {

                                batchMedianTimer.Start();
                                var medianImage = ImageProcessing.MedianFilter3x3(grayscaleImage, MaskWeights3x3);

                                batchMedianTimer.Stop();
                                var medianImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_median_3x3" + file.Extension);
                                medianImage.Save(medianImageName);


                        }

                        if (filterSelection == 4)
                        {


                                batchMedianTimer.Start();
                                var medianImage = ImageProcessing.MedianFilter5x5(grayscaleImage, MaskWeights5x5);

                                batchMedianTimer.Stop();
                                var medianImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_median_5x5" + file.Extension);
                                medianImage.Save(medianImageName);


                        }

                        if (filterSelection == 5)
                        {

                            batchLinearTimer.Start();
                            var linearImage = ImageProcessing.LinearFilter5x5(grayscaleImage, MaskWeights5x5, factor, bias);

                            batchLinearTimer.Stop();
                            var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_5x5_gaussian_blur" + file.Extension);
                            linearImage.Save(linearImageName);

                        }

                        //Begin Image Quantization
                        var quantizedImageError = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_quantization_error.txt");

                        batchQuantizationTimer.Start();
                            var quantizedImage = ImageProcessing.UniformImageQuantization(grayscaleImage, quantInput, quantizedImageError);
                            batchQuantizationTimer.Stop();
                            var quantizedImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_quantized_" + quantLevelSlider.Value.ToString() + "_levels" + file.Extension);
                            quantizedImage.Save(quantizedImageName);

                        
 

                    }
                }
            }
            ImageProcessing.GenerateCellClassAvgHistogram("cyl");
            ImageProcessing.GenerateCellClassAvgHistogram("inter");

            ImageProcessing.GenerateCellClassAvgHistogram("para");

            ImageProcessing.GenerateCellClassAvgHistogram("mod");

            ImageProcessing.GenerateCellClassAvgHistogram("super");

            ImageProcessing.GenerateCellClassAvgHistogram("svar");
            ImageProcessing.GenerateCellClassAvgHistogram("let");

            totaltimer.Stop();

            #region Metrics
            batchGrayscaleTimeBox.Text = batchGrayscaletimer.ElapsedMilliseconds.ToString() + " ms";
            BatchImpulseTimeBox.Text = batchImpulseNoiseTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchGaussianNoiseTimeBox.Text = batchGaussianNoiseTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchHistogramCalculationTimeBox.Text = batchHistGenTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchHistEqTimeBox.Text = batchHistEqImageTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchQuantizationTimeBox.Text = batchQuantizationTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchSharpenTimeBox.Text = batchLinearTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchMedianFilterTimeBox.Text = batchMedianTimer.ElapsedMilliseconds.ToString() + " ms";

            avgGrayScaleBox.Text = (batchGrayscaletimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgImpulseBox.Text = (batchImpulseNoiseTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgGNoiseBox.Text = (batchGaussianNoiseTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgHistCalcBox.Text = (batchHistGenTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgHistEqBox.Text = (batchHistEqImageTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgQuantBox.Text = (batchQuantizationTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgLinearBox.Text = (batchLinearTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgMedianBox.Text = (batchMedianTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            totalTimeBox.Text = totaltimer.ElapsedMilliseconds.ToString() + " ms";
            #endregion
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            inputTextBox.ReadOnly = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            outputTextBox.ReadOnly = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void gaussian_mean_TextChanged(object sender, EventArgs e)
        {

        }

        private void eqHistogram_Click(object sender, EventArgs e)
        {

        }

        private void rgb2g_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default["ColorChoice"] = rgb2g.Tag;
            Properties.Settings.Default.Save();
        }

        private void b2g_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default["ColorChoice"] = b2g.Tag;
            Properties.Settings.Default.Save();
        }

        private void g2g_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default["ColorChoice"] = g2g.Tag;
            Properties.Settings.Default.Save();
        }

        private void r2g_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default["ColorChoice"] = r2g.Tag;
            Properties.Settings.Default.Save();
        }

        private void hiseqimage_Click(object sender, EventArgs e)
        {

        }

        private void gaussianNoiseSlider_Scroll(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Matrix3x3.Visible = false;
                Matrix5x5.Visible = false;
                filterBiasTextBox.Text = "0.0";
                filterFactorTextBox.Text = "0.0";
                filterBiasTextBox.Enabled = false;
                filterFactorTextBox.Enabled = false;

            }
            if (comboBox1.SelectedIndex == 1)
            {
                Matrix3x3.Visible = true;
                Matrix5x5.Visible = false;
                M3x300.Text = "0";
                M3x301.Text = "-1";
                M3x302.Text = "0";
                M3x310.Text = "-1";
                M3x311.Text = "5";
                M3x312.Text = "-1";
                M3x320.Text = "0";
                M3x321.Text = "-1";
                M3x322.Text = "0";

                filterFactorTextBox.Text = "1.0";
                filterBiasTextBox.Text = "0.0";
                filterBiasTextBox.Enabled = true;
                filterFactorTextBox.Enabled = true;

            }
            if (comboBox1.SelectedIndex == 2)
            {
                Matrix3x3.Visible = false;
                Matrix5x5.Visible = true;

                //Sharpen linear 5x5 preset
                Matrix5x500.Text = "-1";
                Matrix5x501.Text = "-1";
                Matrix5x502.Text = "-1";
                Matrix5x503.Text = "-1";
                Matrix5x504.Text = "-1";
                Matrix5x510.Text = "-1";
                Matrix5x511.Text = "-1";
                Matrix5x512.Text = "-1";
                Matrix5x513.Text = "-1";
                Matrix5x514.Text = "-1";
                Matrix5x520.Text = "-1";
                Matrix5x521.Text = "-1";
                Matrix5x522.Text = "25";
                Matrix5x523.Text = "-1";
                Matrix5x524.Text = "-1";
                Matrix5x530.Text = "-1";
                Matrix5x531.Text = "-1";
                Matrix5x532.Text = "-1";
                Matrix5x533.Text = "-1";
                Matrix5x534.Text = "-1";
                Matrix5x540.Text = "-1";
                Matrix5x541.Text = "-1";
                Matrix5x542.Text = "-1";
                Matrix5x543.Text = "-1";
                Matrix5x544.Text = "-1";

                filterFactorTextBox.Text = "1.0";
                filterBiasTextBox.Text = "0.0";
                filterBiasTextBox.Enabled = true;
                filterFactorTextBox.Enabled = true;
            }

            if (comboBox1.SelectedIndex == 3)
            {
                Matrix3x3.Visible = true;
                Matrix5x5.Visible = false;
                M3x300.Text = "1";
                M3x301.Text = "1";
                M3x302.Text = "1";
                M3x310.Text = "1";
                M3x311.Text = "1";
                M3x312.Text = "1";
                M3x320.Text = "1";
                M3x321.Text = "1";
                M3x322.Text = "1";

                filterFactorTextBox.Text = "0.0";
                filterBiasTextBox.Text = "0.0";
                filterBiasTextBox.Enabled = false;
                filterFactorTextBox.Enabled = false;

            }

            if (comboBox1.SelectedIndex == 4)
            {
                Matrix3x3.Visible = false;
                Matrix5x5.Visible = true;

                Matrix5x500.Text = "1";
                Matrix5x501.Text = "1";
                Matrix5x502.Text = "1";
                Matrix5x503.Text = "1";
                Matrix5x504.Text = "1";
                Matrix5x510.Text = "1";
                Matrix5x511.Text = "1";
                Matrix5x512.Text = "1";
                Matrix5x513.Text = "1";
                Matrix5x514.Text = "1";
                Matrix5x520.Text = "1";
                Matrix5x521.Text = "1";
                Matrix5x522.Text = "1";
                Matrix5x523.Text = "1";
                Matrix5x524.Text = "1";
                Matrix5x530.Text = "1";
                Matrix5x531.Text = "1";
                Matrix5x532.Text = "1";
                Matrix5x533.Text = "1";
                Matrix5x534.Text = "1";
                Matrix5x540.Text = "1";
                Matrix5x541.Text = "1";
                Matrix5x542.Text = "1";
                Matrix5x543.Text = "1";
                Matrix5x544.Text = "1";

                filterFactorTextBox.Text = "0.0";
                filterBiasTextBox.Text = "0.0";
                filterBiasTextBox.Enabled = false;
                filterFactorTextBox.Enabled = false;
            }

            if (comboBox1.SelectedIndex == 5)
            {
                Matrix3x3.Visible = false;
                Matrix5x5.Visible = true;


                //Gaussian blur preset for 5x5
                Matrix5x500.Text = "1";
                Matrix5x501.Text = "4";
                Matrix5x502.Text = "6";
                Matrix5x503.Text = "4";
                Matrix5x504.Text = "1";
                Matrix5x510.Text = "4";
                Matrix5x511.Text = "16";
                Matrix5x512.Text = "24";
                Matrix5x513.Text = "16";
                Matrix5x514.Text = "4";
                Matrix5x520.Text = "6";
                Matrix5x521.Text = "24";
                Matrix5x522.Text = "36";
                Matrix5x523.Text = "24";
                Matrix5x524.Text = "6";
                Matrix5x530.Text = "4";
                Matrix5x531.Text = "16";
                Matrix5x532.Text = "24";
                Matrix5x533.Text = "16";
                Matrix5x534.Text = "4";
                Matrix5x540.Text = "1";
                Matrix5x541.Text = "4";
                Matrix5x542.Text = "6";
                Matrix5x543.Text = "4";
                Matrix5x544.Text = "1";

                filterFactorTextBox.Text = "0.00390625";
                filterBiasTextBox.Text = "0.0";
                filterBiasTextBox.Enabled = true;
                filterFactorTextBox.Enabled = true;
            }

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void quantLevelSlider_Scroll(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int filterSelection = comboBox1.SelectedIndex;
            double factor = Double.Parse(filterFactorTextBox.Text);
            double bias = Double.Parse(filterBiasTextBox.Text);
            int impulseInput = impulsenoiseintensityslider.Value;
            int gaussianInput = gaussianNoiseSlider.Value;
            int quantInput = quantLevelSlider.Value;

            List<double> MaskWeights3x3 = new List<double>();
            List<double> MaskWeights5x5 = new List<double>();
            if (filterSelection == 1 || filterSelection == 3)
            {
                MaskWeights3x3.Add(double.Parse(M3x300.Text));
                MaskWeights3x3.Add(double.Parse(M3x301.Text));
                MaskWeights3x3.Add(double.Parse(M3x302.Text));
                MaskWeights3x3.Add(double.Parse(M3x310.Text));
                MaskWeights3x3.Add(double.Parse(M3x311.Text));
                MaskWeights3x3.Add(double.Parse(M3x312.Text));
                MaskWeights3x3.Add(double.Parse(M3x320.Text));
                MaskWeights3x3.Add(double.Parse(M3x321.Text));
                MaskWeights3x3.Add(double.Parse(M3x322.Text));
            }

            if (filterSelection == 2 || filterSelection == 4 || filterSelection == 5)
            {
                MaskWeights5x5.Add(double.Parse(Matrix5x500.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x501.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x502.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x503.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x504.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x510.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x511.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x512.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x513.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x514.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x520.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x521.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x522.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x523.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x524.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x530.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x531.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x532.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x533.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x534.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x540.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x541.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x542.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x543.Text));
                MaskWeights5x5.Add(double.Parse(Matrix5x544.Text));
            }
            var totaltimer = new Stopwatch();
            totaltimer.Start();
            string inputPath = Settings.Default["InputPath"].ToString();
            string outputPath = Settings.Default["OutputPath"].ToString();
            var files = new DirectoryInfo(inputPath)
            .GetFiles()
            .Where(f => f.IsImage());

            if (String.IsNullOrWhiteSpace(inputPath) && String.IsNullOrWhiteSpace(outputPath))
            {
                button5.Enabled = false;
            }
            var batchGrayscaletimer = new Stopwatch();
            var batchImpulseNoiseTimer = new Stopwatch();
            var batchGaussianNoiseTimer = new Stopwatch();
            var batchHistGenTimer = new Stopwatch();
            var batchHistEqImageTimer = new Stopwatch();
            var batchQuantizationTimer = new Stopwatch();
            var batchLinearTimer = new Stopwatch();
            var batchMedianTimer = new Stopwatch();


            //parallelized method. Extremely fast. 
            Parallel.ForEach(files, file =>
            {
                using (Bitmap image = (Bitmap)Bitmap.FromFile(file.FullName))
                {
                    batchGrayscaletimer.Start();
                    using (var grayscaleImage = ImageProcessing.RGB2GrayscaleImage(image, Settings.Default.ColorChoice))
                    //using (var grayscaleImageBuffer = ImageProcessing.RGB2GrayscaleImage(image, Settings.Default.ColorChoice))
                    {
                        batchGrayscaletimer.Stop();
                        var grayscaleImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_grayscale_conversion" + file.Extension);
                        grayscaleImage.Save(grayscaleImageName);

                        if (impulseInput > 0)
                        {

                                batchImpulseNoiseTimer.Start();
                                var ImpulsedImage = ImageProcessing.ImpulseNoise(grayscaleImage, impulseInput);
                                batchImpulseNoiseTimer.Stop();
                                var newImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_processed_impulse_noise" + file.Extension);
                                ImpulsedImage.Save(newImageName);
                        }

                        if (gaussianInput > 0)
                        {

                                batchGaussianNoiseTimer.Start();
                                var GaussianNoiseImage = ImageProcessing.GaussianNoise(grayscaleImage, gaussianInput);
                                batchGaussianNoiseTimer.Stop();
                                var newImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_processed_gaussian_noise" + file.Extension);
                                GaussianNoiseImage.Save(newImageName);
                        }

                            batchHistGenTimer.Start();
                            var HistogramImage = ImageProcessing.GenerateHistogram(grayscaleImage);
                            batchHistGenTimer.Stop();
                            var histogramImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_histogram" + file.Extension);
                            HistogramImage.Save(histogramImageName);

                            batchHistEqImageTimer.Start();
                            var HistogramEqImage = ImageProcessing.HistogramEqualizedImage(grayscaleImage);
                            batchHistEqImageTimer.Stop();

                            var histogramEqImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_histogram_equalized" + file.Extension);
                            HistogramEqImage.Save(histogramEqImageName);


                        var quantizedImageError = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_quantization_error.txt");

                        batchQuantizationTimer.Start();
                            var quantizedImage = ImageProcessing.UniformImageQuantization(grayscaleImage, quantInput, quantizedImageError);

                            batchQuantizationTimer.Stop();
                            var quantizedImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_quantized_" + quantInput.ToString() + "_levels" + file.Extension);
                            quantizedImage.Save(quantizedImageName);

                        if (filterSelection == 1)
                        {
                                batchLinearTimer.Start();
                                var linearImage = ImageProcessing.LinearFilter3x3(grayscaleImage, MaskWeights3x3, factor, bias);

                                batchLinearTimer.Stop();
                                var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_3x3_sharpen" + file.Extension);
                                linearImage.Save(linearImageName);

                        }

                        if (filterSelection == 2)
                        {
           
                                batchLinearTimer.Start();
                                var linearImage = ImageProcessing.LinearFilter5x5(grayscaleImage, MaskWeights5x5, factor, bias);

                                batchLinearTimer.Stop();
                                var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_5x5_sharpen" + file.Extension);
                                linearImage.Save(linearImageName);

                        }

                        if (filterSelection == 3)
                        {

                                batchMedianTimer.Start();
                                var medianImage = ImageProcessing.MedianFilter3x3(grayscaleImage, MaskWeights3x3);

                                batchMedianTimer.Stop();
                                var medianImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_median_3x3" + file.Extension);
                                medianImage.Save(medianImageName);

                        }

                        if (filterSelection == 4)
                        {

                                batchMedianTimer.Start();
                                var medianImage = ImageProcessing.MedianFilter5x5(grayscaleImage, MaskWeights5x5);

                                batchMedianTimer.Stop();
                                var medianImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_median_5x5" + file.Extension);
                                medianImage.Save(medianImageName);
                        }

                        if (filterSelection == 5)
                        {

                            batchLinearTimer.Start();
                            var linearImage = ImageProcessing.LinearFilter5x5(grayscaleImage, MaskWeights5x5, factor, bias);

                            batchLinearTimer.Stop();
                            var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_5x5_gaussian_blur" + file.Extension);
                            linearImage.Save(linearImageName);

                        }

                    }
                }
            });

            ImageProcessing.GenerateCellClassAvgHistogram("cyl");
            ImageProcessing.GenerateCellClassAvgHistogram("inter");

            ImageProcessing.GenerateCellClassAvgHistogram("para");

            ImageProcessing.GenerateCellClassAvgHistogram("mod");

            ImageProcessing.GenerateCellClassAvgHistogram("super");

            ImageProcessing.GenerateCellClassAvgHistogram("svar");
            ImageProcessing.GenerateCellClassAvgHistogram("let");

            totaltimer.Stop();

            #region Metrics
            batchGrayscaleTimeBox.Text = batchGrayscaletimer.ElapsedMilliseconds.ToString() + " ms";
            BatchImpulseTimeBox.Text = batchImpulseNoiseTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchGaussianNoiseTimeBox.Text = batchGaussianNoiseTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchHistogramCalculationTimeBox.Text = batchHistGenTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchHistEqTimeBox.Text = batchHistEqImageTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchQuantizationTimeBox.Text = batchQuantizationTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchSharpenTimeBox.Text = batchLinearTimer.ElapsedMilliseconds.ToString() + " ms";
            BatchMedianFilterTimeBox.Text = batchMedianTimer.ElapsedMilliseconds.ToString() + " ms";

            avgGrayScaleBox.Text = (batchGrayscaletimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgImpulseBox.Text = (batchImpulseNoiseTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgGNoiseBox.Text = (batchGaussianNoiseTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgHistCalcBox.Text = (batchHistGenTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgHistEqBox.Text = (batchHistEqImageTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgQuantBox.Text = (batchQuantizationTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgLinearBox.Text = (batchLinearTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            avgMedianBox.Text = (batchMedianTimer.ElapsedMilliseconds / files.Count()).ToString() + " ms";
            totalTimeBox.Text = totaltimer.ElapsedMilliseconds.ToString() + " ms";
            #endregion
        }

        private void label37_Click(object sender, EventArgs e)
        {

        }
    }
    }
