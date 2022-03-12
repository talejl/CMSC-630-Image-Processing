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
            quantErrorBox.ReadOnly = true;
            linearFilterSelectionBox.Items.Insert(0, "None");
            linearFilterSelectionBox.Items.Insert(1, "Linear 3x3 (Sharpen Preset)");
            linearFilterSelectionBox.Items.Insert(2, "Linear 5x5 (Sharpen Preset)");
            linearFilterSelectionBox.Items.Insert(3, "Linear 5x5 (Gaussian Blur Preset)");
            medianFilterSelectionBox.Items.Insert(0, "None");
            medianFilterSelectionBox.Items.Insert(1, "Median 3x3");
            medianFilterSelectionBox.Items.Insert(2, "Median 5x5");
            filterBiasTextBox.Text = "0.0";
            filterFactorTextBox.Text = "1.0";
            linearFilterSelectionBox.SelectedIndex = 0;
            medianFilterSelectionBox.SelectedIndex = 0;
            quantLevelSlider.Value = 8;
            Matrix3x3.Visible = false;
            Matrix5x5.Visible = false;
            MedianMatrix3x3.Visible = false;
            MedianMatrix5x5.Visible = false;
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
            int linearFilterSelection = linearFilterSelectionBox.SelectedIndex;
            int medianFilterSelection = medianFilterSelectionBox.SelectedIndex;
            double factor = Double.Parse(filterFactorTextBox.Text);
            double bias = Double.Parse(filterBiasTextBox.Text);
            int impulseInput = impulsenoiseintensityslider.Value;
            int gaussianInput = gaussianNoiseSlider.Value;
            int quantInput = quantLevelSlider.Value;
            //List<string> cellClasses = new List<string> { "cyl", "para", "inter", "let", "mod", "super", "svar" };

            List<double> MaskWeights3x3 = new List<double>();
            List<double> MaskWeights5x5 = new List<double>();
            List<double> MedianMaskWeights3x3 = new List<double>();
            List<double> MedianMaskWeights5x5 = new List<double>();

            if (linearFilterSelection == 1)
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

            if (medianFilterSelection == 1)
            {
                MedianMaskWeights3x3.Add(double.Parse(MM3x300.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x301.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x302.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x310.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x311.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x312.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x320.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x321.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x322.Text));
            }

            if (linearFilterSelection == 2 || linearFilterSelection == 3)
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

            if (medianFilterSelection == 2)
            {
                MedianMaskWeights5x5.Add(double.Parse(MM5x500.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x501.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x502.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x503.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x504.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x510.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x511.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x512.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x513.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x514.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x520.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x521.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x522.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x523.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x524.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x530.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x531.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x532.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x533.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x534.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x540.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x541.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x542.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x543.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x544.Text));
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
            grayscaletimer.Stop();
            this.Modified.Image = ImageProcessing.ResizeImage(modifiedImage, 288, 219);


            histEqImageTimer.Start();
            //Bitmap eqImg = ImageProcessing.HistogramEqualizedImage(modifiedImage);
            Bitmap eqImg = ImageProcessing.HistogramEqualizedImage(modifiedImage);
            histEqImageTimer.Stop();

            if (impulseInput > 0)
            {
                impulseNoiseTimer.Start();
                Bitmap ImpulseNoiseImage = ImageProcessing.ImpulseNoise(modifiedImage, impulseInput);
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


            if(linearFilterSelection == 1)
            {
                linearTimer.Start();
                Bitmap linearImage = ImageProcessing.LinearFilter3x3(modifiedImage, MaskWeights3x3, factor, bias);
                linearTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(linearImage, 288, 219);
            }

            if (linearFilterSelection == 2)
            {
                linearTimer.Start();
                Bitmap linearImage5x5 = ImageProcessing.LinearFilter5x5(modifiedImage, MaskWeights5x5, factor, bias);
                linearTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(linearImage5x5, 288, 219);
            }
            if (medianFilterSelection == 1)
            {
                medianFilterTimer.Start();
                Bitmap MedianFilteredImage = ImageProcessing.MedianFilter3x3(modifiedImage, MedianMaskWeights3x3);
                medianFilterTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(MedianFilteredImage, 288, 219);
            }

            if (medianFilterSelection == 2)
            {
                medianFilterTimer.Start();
                Bitmap MedianFilteredImage = ImageProcessing.MedianFilter5x5(modifiedImage, MedianMaskWeights5x5);
                medianFilterTimer.Stop();
                this.Modified.Image = ImageProcessing.ResizeImage(MedianFilteredImage, 288, 219);
            }

            if (linearFilterSelection == 3)
            {
                linearTimer.Start();
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
            int linearFilterSelection = linearFilterSelectionBox.SelectedIndex;
            int medianFilterSelection = medianFilterSelectionBox.SelectedIndex;

            double factor = Double.Parse(filterFactorTextBox.Text);
            double bias = Double.Parse(filterBiasTextBox.Text);
            int impulseInput = impulsenoiseintensityslider.Value;
            int gaussianInput = gaussianNoiseSlider.Value;
            int quantInput = quantLevelSlider.Value;
            List<string> cellClasses = new List<string> { "cyl", "para", "inter", "let", "mod", "super", "svar" };
            List<double> MaskWeights3x3 = new List<double>();
            List<double> MaskWeights5x5 = new List<double>();
            List<double> MedianMaskWeights3x3 = new List<double>();
            List<double> MedianMaskWeights5x5 = new List<double>();
            #region placeholders for stopwatch

            long batchGrayscaletimerInt = 0;
            long batchImpulseNoiseTimerInt = 0;
            long batchGaussianNoiseTimerInt = 0;
            long batchHistGenTimerInt = 0;
            long batchClassHistGenTimerInt = 0;
            long batchHistEqImageTimerInt = 0;
            long batchQuantizationTimerInt = 0;
            long batchLinearTimerInt = 0;
            long batchMedianTimerInt = 0;
            #endregion
            #region Filter Mask Weight Mapping


            if (linearFilterSelection == 1)
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

            if (medianFilterSelection == 1)
            {
                MedianMaskWeights3x3.Add(double.Parse(MM3x300.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x301.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x302.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x310.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x311.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x312.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x320.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x321.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x322.Text));
            }

            if (linearFilterSelection == 2 || linearFilterSelection == 3)
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

            if (medianFilterSelection == 2)
            {
                MedianMaskWeights5x5.Add(double.Parse(MM5x500.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x501.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x502.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x503.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x504.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x510.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x511.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x512.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x513.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x514.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x520.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x521.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x522.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x523.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x524.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x530.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x531.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x532.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x533.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x534.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x540.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x541.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x542.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x543.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x544.Text));
            }
            #endregion

            string inputPath = Settings.Default["InputPath"].ToString();
            string outputPath = Settings.Default["OutputPath"].ToString();
            var files = new DirectoryInfo(inputPath)
            .GetFiles()
            .Where(f => f.IsImage());

            if(String.IsNullOrWhiteSpace(inputPath) && String.IsNullOrWhiteSpace(outputPath))
            {
                button5.Enabled = false;
            }
            #region Initialize timers
            var batchGrayscaleTimer = new Stopwatch();
            var batchImpulseNoiseTimer = new Stopwatch();
            var batchGaussianNoiseTimer = new Stopwatch();
            var batchHistGenTimer = new Stopwatch();
            var batchClassHistGenTimer = new Stopwatch();
            var batchHistEqImageTimer = new Stopwatch();
            var batchQuantizationTimer = new Stopwatch();
            var batchLinearTimer = new Stopwatch();
            var batchMedianTimer = new Stopwatch();
            var totaltimer = new Stopwatch();
            totaltimer.Start();
            #endregion

            //Original single threaded execution process
            foreach (var file in files)
            {
                using (Bitmap image = (Bitmap)Bitmap.FromFile(file.FullName))
                {
                    batchGrayscaleTimer.Restart();
                    var grayscaleImage = ImageProcessing.RGB2GrayscaleImage(image, Settings.Default.ColorChoice);
                    var grayscaleImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_grayscale_conversion" + file.Extension);
                    grayscaleImage.Save(grayscaleImageName);
                    batchGrayscaleTimer.Stop();
                    batchGrayscaletimerInt += batchGrayscaleTimer.ElapsedMilliseconds;

                    //Begin Impulse Noise
                    if (impulseInput > 0)
                        {
                                batchImpulseNoiseTimer.Restart();
                                //batchImpulseNoiseTimer.Start();
                                var ImpulsedImage = ImageProcessing.ImpulseNoise(grayscaleImage, impulseInput);
                                var newImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_processed_impulse_noise" + file.Extension);
                                ImpulsedImage.Save(newImageName);
                                batchImpulseNoiseTimer.Stop();
                                batchImpulseNoiseTimerInt += batchImpulseNoiseTimer.ElapsedMilliseconds;
                    }
                    //Begin Gaussian Noise 
                    if (gaussianInput > 0)
                        {

                                //batchGaussianNoiseTimer.Start();
                                batchGaussianNoiseTimer.Restart();
                                var GaussianNoiseImage = ImageProcessing.GaussianNoise(grayscaleImage, gaussianInput);
                                var newImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_processed_gaussian_noise" + file.Extension);
                                GaussianNoiseImage.Save(newImageName);
                                batchGaussianNoiseTimer.Stop();
                                batchGaussianNoiseTimerInt += batchGaussianNoiseTimer.ElapsedMilliseconds;
                        }


                            //Begin histogram generation
                            //batchHistGenTimer.Start();
                            batchHistGenTimer.Restart();
                            var HistogramImage = ImageProcessing.GenerateHistogram(grayscaleImage);
                            var histogramImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_histogram" + file.Extension);
                            HistogramImage.Save(histogramImageName);
                            batchHistGenTimer.Stop();
                            batchHistGenTimerInt += batchHistGenTimer.ElapsedMilliseconds;

                            //Begin histogram equalization
                            //batchHistEqImageTimer.Start();
                            batchHistEqImageTimer.Restart();
                            var HistogramEqImage = ImageProcessing.HistogramEqualizedImage(grayscaleImage);
                            var histogramEqImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_histogram_equalized" + file.Extension);
                            HistogramEqImage.Save(histogramEqImageName);
                            batchHistEqImageTimer.Stop();
                            batchHistEqImageTimerInt+= batchHistEqImageTimer.ElapsedMilliseconds;


                        //Begin Filtering
                        if (linearFilterSelection == 1)
                        {

                                //batchLinearTimer.Start();
                                batchLinearTimer.Restart();
                                var linearImage = ImageProcessing.LinearFilter3x3(grayscaleImage, MaskWeights3x3, factor, bias);
                                var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_3x3_sharpen" + file.Extension);
                                linearImage.Save(linearImageName);
                                batchLinearTimer.Stop();
                                batchLinearTimerInt += batchLinearTimer.ElapsedMilliseconds;
                        }

                    if (linearFilterSelection == 2)
                        {
                            
                                //batchLinearTimer.Start();
                                batchLinearTimer.Restart();
                                var linearImage = ImageProcessing.LinearFilter5x5(grayscaleImage, MaskWeights5x5, factor, bias);
                                var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_5x5_sharpen" + file.Extension);
                                linearImage.Save(linearImageName);
                                batchLinearTimer.Stop();
                                batchLinearTimerInt += batchLinearTimer.ElapsedMilliseconds;
                        }

                        if (medianFilterSelection == 1)
                        {
                                //batchMedianTimer.Start();
                                batchMedianTimer.Restart();
                                var medianImage = ImageProcessing.MedianFilter3x3(grayscaleImage, MedianMaskWeights3x3);
                                var medianImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_median_3x3" + file.Extension);
                                medianImage.Save(medianImageName);
                                batchMedianTimer.Stop();
                                batchMedianTimerInt += batchMedianTimer.ElapsedMilliseconds;
                        }

                    if (medianFilterSelection == 2)
                        {

                                batchMedianTimer.Restart();
                                //batchMedianTimer.Start();
                                var medianImage = ImageProcessing.MedianFilter5x5(grayscaleImage, MedianMaskWeights5x5);
                                var medianImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_median_5x5" + file.Extension);
                                medianImage.Save(medianImageName);
                                batchMedianTimer.Stop();
                        batchMedianTimerInt += batchMedianTimer.ElapsedMilliseconds;

                    }

                    if (linearFilterSelection == 3)
                        {

                            //batchLinearTimer.Start();
                            batchLinearTimer.Restart();
                            var linearImage = ImageProcessing.LinearFilter5x5(grayscaleImage, MaskWeights5x5, factor, bias);
                            var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_5x5_gaussian_blur" + file.Extension);
                            linearImage.Save(linearImageName);
                            batchLinearTimer.Stop();
                            batchLinearTimerInt += batchLinearTimer.ElapsedMilliseconds;
                        }

                            //Begin Image Quantization
                            var quantizedImageError = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_quantization_error.txt");
                            //batchQuantizationTimer.Start();
                            batchQuantizationTimer.Restart();
                            var quantizedImage = ImageProcessing.UniformImageQuantization(grayscaleImage, quantInput, quantizedImageError);
                            var quantizedImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_quantized_" + quantInput.ToString() + "_levels" + file.Extension);
                            quantizedImage.Save(quantizedImageName);
                            batchQuantizationTimer.Stop();
                            batchQuantizationTimerInt += batchQuantizationTimer.ElapsedMilliseconds; 
                }
            }

            batchClassHistGenTimer.Restart();
            foreach(var cellClass in cellClasses)
            {
                ImageProcessing.GenerateCellClassAvgHistogram(cellClass);
            }
            batchClassHistGenTimer.Stop();
            batchClassHistGenTimerInt += batchClassHistGenTimer.ElapsedMilliseconds;

            totaltimer.Stop();

            #region Metrics
            batchGrayscaleTimeBox.Text = batchGrayscaletimerInt.ToString() + " ms";
            BatchImpulseTimeBox.Text = batchImpulseNoiseTimerInt.ToString() + " ms";
            BatchGaussianNoiseTimeBox.Text = batchGaussianNoiseTimerInt.ToString() + " ms";
            BatchHistogramCalculationTimeBox.Text = batchHistGenTimerInt.ToString() + " ms";
            BatchHistEqTimeBox.Text = batchHistEqImageTimerInt.ToString() + " ms";
            BatchQuantizationTimeBox.Text = batchQuantizationTimerInt.ToString() + " ms";
            BatchSharpenTimeBox.Text = batchLinearTimerInt.ToString() + " ms";
            BatchMedianFilterTimeBox.Text = batchMedianTimerInt.ToString() + " ms";
            classHist.Text = batchClassHistGenTimerInt.ToString() + " ms";
            avgGrayScaleBox.Text = (batchGrayscaletimerInt / files.Count()).ToString() + " ms";
            avgImpulseBox.Text = (batchImpulseNoiseTimerInt / files.Count()).ToString() + " ms";
            avgGNoiseBox.Text = (batchGaussianNoiseTimerInt / files.Count()).ToString() + " ms";
            avgHistCalcBox.Text = (batchHistGenTimerInt / files.Count()).ToString() + " ms";
            avgHistEqBox.Text = (batchHistEqImageTimerInt / files.Count()).ToString() + " ms";
            avgQuantBox.Text = (batchQuantizationTimerInt / files.Count()).ToString() + " ms";
            avgLinearBox.Text = (batchLinearTimerInt / files.Count()).ToString() + " ms";
            avgMedianBox.Text = (batchMedianTimerInt / files.Count()).ToString() + " ms";
            avgClassHist.Text = (batchClassHistGenTimerInt / 7).ToString() + " ms";
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
            if (linearFilterSelectionBox.SelectedIndex == 0)
            {
                Matrix3x3.Visible = false;
                Matrix5x5.Visible = false;
                filterBiasTextBox.Text = "0.0";
                filterFactorTextBox.Text = "0.0";
                filterBiasTextBox.Enabled = false;
                filterFactorTextBox.Enabled = false;

            }
            if (linearFilterSelectionBox.SelectedIndex == 1)
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
            if (linearFilterSelectionBox.SelectedIndex == 2)
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


            if (linearFilterSelectionBox.SelectedIndex == 3)
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
            int linearFilterSelection = linearFilterSelectionBox.SelectedIndex;
            int medianFilterSelection = medianFilterSelectionBox.SelectedIndex;
            double factor = Double.Parse(filterFactorTextBox.Text);
            double bias = Double.Parse(filterBiasTextBox.Text);
            int impulseInput = impulsenoiseintensityslider.Value;
            int gaussianInput = gaussianNoiseSlider.Value;
            int quantInput = quantLevelSlider.Value;
            List<string> cellClasses = new List<string> {"cyl", "para", "inter", "let", "mod", "super", "svar" };
            List<double> MaskWeights3x3 = new List<double>();
            List<double> MaskWeights5x5 = new List<double>();
            List<double> MedianMaskWeights3x3 = new List<double>();
            List<double> MedianMaskWeights5x5 = new List<double>();

            #region placeholders for stopwatch

            long batchGrayscaletimerInt = 0;
            long batchImpulseNoiseTimerInt = 0;
            long batchGaussianNoiseTimerInt = 0;
            long batchHistGenTimerInt = 0;
            long batchClassHistGenTimerInt = 0;
            long batchHistEqImageTimerInt = 0;
            long batchQuantizationTimerInt = 0;
            long batchLinearTimerInt = 0;
            long batchMedianTimerInt = 0;
            #endregion

            #region Filter mask weight mapping
            if (linearFilterSelection == 1)
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

            if (medianFilterSelection == 1)
            {
                MedianMaskWeights3x3.Add(double.Parse(MM3x300.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x301.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x302.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x310.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x311.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x312.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x320.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x321.Text));
                MedianMaskWeights3x3.Add(double.Parse(MM3x322.Text));
            }

            if (linearFilterSelection == 2 || linearFilterSelection == 3)
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

            if (medianFilterSelection == 2)
            {
                MedianMaskWeights5x5.Add(double.Parse(MM5x500.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x501.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x502.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x503.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x504.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x510.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x511.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x512.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x513.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x514.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x520.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x521.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x522.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x523.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x524.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x530.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x531.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x532.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x533.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x534.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x540.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x541.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x542.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x543.Text));
                MedianMaskWeights5x5.Add(double.Parse(MM5x544.Text));
            }
            #endregion

            string inputPath = Settings.Default["InputPath"].ToString();
            string outputPath = Settings.Default["OutputPath"].ToString();
            var files = new DirectoryInfo(inputPath)
            .GetFiles()
            .Where(f => f.IsImage());

            if (String.IsNullOrWhiteSpace(inputPath) && String.IsNullOrWhiteSpace(outputPath))
            {
                button5.Enabled = false;
            }
            var batchGrayscaleTimer = new Stopwatch();
            var batchImpulseNoiseTimer = new Stopwatch();
            var batchGaussianNoiseTimer = new Stopwatch();
            var batchHistGenTimer = new Stopwatch();
            var batchHistEqImageTimer = new Stopwatch();
            var batchClassHistGenTimer = new Stopwatch();
            var batchQuantizationTimer = new Stopwatch();
            var batchLinearTimer = new Stopwatch();
            var batchMedianTimer = new Stopwatch();
            var totaltimer = new Stopwatch();
            totaltimer.Start();

            //parallelized method. Extremely fast. 
            Parallel.ForEach(files, file =>
            {
                using (Bitmap image = (Bitmap)Bitmap.FromFile(file.FullName))
                {
                    batchGrayscaleTimer.Restart();
                    var grayscaleImage = ImageProcessing.RGB2GrayscaleImage(image, Settings.Default.ColorChoice);
                    var grayscaleImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_grayscale_conversion" + file.Extension);
                    grayscaleImage.Save(grayscaleImageName);
                    batchGrayscaleTimer.Stop();
                    batchGrayscaletimerInt += batchGrayscaleTimer.ElapsedMilliseconds;

                    //Begin Impulse Noise
                    if (impulseInput > 0)
                    {
                        batchImpulseNoiseTimer.Restart();
                        var ImpulsedImage = ImageProcessing.ImpulseNoise(grayscaleImage, impulseInput);
                        batchImpulseNoiseTimer.Stop();
                        batchImpulseNoiseTimerInt += batchImpulseNoiseTimer.ElapsedMilliseconds;
                        var newImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_processed_impulse_noise" + file.Extension);
                        ImpulsedImage.Save(newImageName);

                    }
                    //Begin Gaussian Noise 
                    if (gaussianInput > 0)
                    {

                        //batchGaussianNoiseTimer.Start();
                        batchGaussianNoiseTimer.Restart();
                        var GaussianNoiseImage = ImageProcessing.GaussianNoise(grayscaleImage, gaussianInput);
                        batchGaussianNoiseTimer.Stop();
                        batchGaussianNoiseTimerInt += batchGaussianNoiseTimer.ElapsedMilliseconds;
                        var newImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_processed_gaussian_noise" + file.Extension);
                        GaussianNoiseImage.Save(newImageName);

                    }


                    //Begin histogram generation
                    //batchHistGenTimer.Start();
                    batchHistGenTimer.Restart();
                    var HistogramImage = ImageProcessing.GenerateHistogram(grayscaleImage);
                    var histogramImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_histogram" + file.Extension);
                    HistogramImage.Save(histogramImageName);
                    batchHistGenTimer.Stop();
                    batchHistGenTimerInt += batchHistGenTimer.ElapsedMilliseconds;

                    //Begin histogram equalization
                    //batchHistEqImageTimer.Start();
                    batchHistEqImageTimer.Restart();
                    var HistogramEqImage = ImageProcessing.HistogramEqualizedImage(grayscaleImage);
                    var histogramEqImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_histogram_equalized" + file.Extension);
                    HistogramEqImage.Save(histogramEqImageName);
                    batchHistEqImageTimer.Stop();
                    batchHistEqImageTimerInt += batchHistEqImageTimer.ElapsedMilliseconds;


                    //Begin Filtering
                    if (linearFilterSelection == 1)
                    {

                        //batchLinearTimer.Start();
                        batchLinearTimer.Restart();
                        var linearImage = ImageProcessing.LinearFilter3x3(grayscaleImage, MaskWeights3x3, factor, bias);
                        var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_3x3_sharpen" + file.Extension);
                        linearImage.Save(linearImageName);
                        batchLinearTimer.Stop();
                        batchLinearTimerInt += batchLinearTimer.ElapsedMilliseconds;
                    }

                    if (linearFilterSelection == 2)
                    {

                        //batchLinearTimer.Start();
                        batchLinearTimer.Restart();
                        var linearImage = ImageProcessing.LinearFilter5x5(grayscaleImage, MaskWeights5x5, factor, bias);
                        var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_5x5_sharpen" + file.Extension);
                        linearImage.Save(linearImageName);
                        batchLinearTimer.Stop();
                        batchLinearTimerInt += batchLinearTimer.ElapsedMilliseconds;
                    }

                    if (medianFilterSelection == 1)
                    {
                        //batchMedianTimer.Start();
                        batchMedianTimer.Restart();
                        var medianImage = ImageProcessing.MedianFilter3x3(grayscaleImage, MedianMaskWeights3x3);
                        var medianImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_median_3x3" + file.Extension);
                        medianImage.Save(medianImageName);
                        batchMedianTimer.Stop();
                        batchMedianTimerInt += batchMedianTimer.ElapsedMilliseconds;
                    }

                    if (medianFilterSelection == 2)
                    {

                        batchMedianTimer.Restart();
                        //batchMedianTimer.Start();
                        var medianImage = ImageProcessing.MedianFilter5x5(grayscaleImage, MedianMaskWeights5x5);
                        var medianImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_median_5x5" + file.Extension);
                        medianImage.Save(medianImageName);
                        batchMedianTimer.Stop();
                        batchMedianTimerInt += batchMedianTimer.ElapsedMilliseconds;

                    }

                    if (linearFilterSelection == 3)
                    {

                        //batchLinearTimer.Start();
                        batchLinearTimer.Restart();
                        var linearImage = ImageProcessing.LinearFilter5x5(grayscaleImage, MaskWeights5x5, factor, bias);
                        var linearImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_linear_5x5_gaussian_blur" + file.Extension);
                        linearImage.Save(linearImageName);
                        batchLinearTimer.Stop();
                        batchLinearTimerInt += batchLinearTimer.ElapsedMilliseconds;
                    }

                    //Begin Image Quantization
                    var quantizedImageError = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_quantization_error.txt");
                    //batchQuantizationTimer.Start();
                    batchQuantizationTimer.Restart();
                    var quantizedImage = ImageProcessing.UniformImageQuantization(grayscaleImage, quantInput, quantizedImageError);
                    var quantizedImageName = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file.Name) + "_quantized_" + quantInput.ToString() + "_levels" + file.Extension);
                    quantizedImage.Save(quantizedImageName);
                    batchQuantizationTimer.Stop();
                    batchQuantizationTimerInt += batchQuantizationTimer.ElapsedMilliseconds;
                }
            });

            Parallel.ForEach(cellClasses, cellClass =>
            {
                batchClassHistGenTimer.Restart();
                ImageProcessing.GenerateCellClassAvgHistogram(cellClass);
                batchClassHistGenTimer.Stop();
                batchClassHistGenTimerInt += batchClassHistGenTimer.ElapsedMilliseconds;
            });


            totaltimer.Stop();

            #region Metrics
            batchGrayscaleTimeBox.Text = batchGrayscaletimerInt.ToString() + " ms";
            BatchImpulseTimeBox.Text = batchImpulseNoiseTimerInt.ToString() + " ms";
            BatchGaussianNoiseTimeBox.Text = batchGaussianNoiseTimerInt.ToString() + " ms";
            BatchHistogramCalculationTimeBox.Text = batchHistGenTimerInt.ToString() + " ms";
            BatchHistEqTimeBox.Text = batchHistEqImageTimerInt.ToString() + " ms";
            BatchQuantizationTimeBox.Text = batchQuantizationTimerInt.ToString() + " ms";
            BatchSharpenTimeBox.Text = batchLinearTimerInt.ToString() + " ms";
            BatchMedianFilterTimeBox.Text = batchMedianTimerInt.ToString() + " ms";
            classHist.Text = batchClassHistGenTimerInt.ToString() + " ms";

            avgGrayScaleBox.Text = (batchGrayscaletimerInt / files.Count()).ToString() + " ms";
            avgImpulseBox.Text = (batchImpulseNoiseTimerInt / files.Count()).ToString() + " ms";
            avgGNoiseBox.Text = (batchGaussianNoiseTimerInt / files.Count()).ToString() + " ms";
            avgHistCalcBox.Text = (batchHistGenTimerInt / files.Count()).ToString() + " ms";
            avgHistEqBox.Text = (batchHistEqImageTimerInt / files.Count()).ToString() + " ms";
            avgQuantBox.Text = (batchQuantizationTimerInt / files.Count()).ToString() + " ms";
            avgLinearBox.Text = (batchLinearTimerInt / files.Count()).ToString() + " ms";
            avgMedianBox.Text = (batchMedianTimerInt / files.Count()).ToString() + " ms";
            avgClassHist.Text = (batchClassHistGenTimerInt / 7).ToString() + " ms";
            totalTimeBox.Text = totaltimer.ElapsedMilliseconds.ToString() + " ms";
            #endregion
        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (medianFilterSelectionBox.SelectedIndex == 0)
            {
                MedianMatrix3x3.Visible = false;
                MedianMatrix5x5.Visible = false;
            }
            if (medianFilterSelectionBox.SelectedIndex == 1)
            {
                MedianMatrix3x3.Visible = true;
                MedianMatrix5x5.Visible = false;
                MM3x300.Text = "1";
                MM3x301.Text = "1";
                MM3x302.Text = "1";
                MM3x310.Text = "1";
                MM3x311.Text = "1";
                MM3x312.Text = "1";
                MM3x320.Text = "1";
                MM3x321.Text = "1";
                MM3x322.Text = "1";
            }

            if (medianFilterSelectionBox.SelectedIndex == 2)
            {
                MedianMatrix3x3.Visible = false;
                MedianMatrix5x5.Visible = true;

                MM5x500.Text = "1";
                MM5x501.Text = "1";
                MM5x502.Text = "1";
                MM5x503.Text = "1";
                MM5x504.Text = "1";
                MM5x510.Text = "1";
                MM5x511.Text = "1";
                MM5x512.Text = "1";
                MM5x513.Text = "1";
                MM5x514.Text = "1";
                MM5x520.Text = "1";
                MM5x521.Text = "1";
                MM5x522.Text = "1";
                MM5x523.Text = "1";
                MM5x524.Text = "1";
                MM5x530.Text = "1";
                MM5x531.Text = "1";
                MM5x532.Text = "1";
                MM5x533.Text = "1";
                MM5x534.Text = "1";
                MM5x540.Text = "1";
                MM5x541.Text = "1";
                MM5x542.Text = "1";
                MM5x543.Text = "1";
                MM5x544.Text = "1";
            }
        }

        private void MM3x300_TextChanged(object sender, EventArgs e)
        {

        }

        private void medianFilterGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }
