using Insta.Graph.Entity;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Insta.Graph.API.Logic
{
    public class ComputerVisionManager
    {

        #region keys
        private string _azureVisionEndpoint = "YOUR AZURE ENDPOINT";
        private string _key = "YOUR AZURE KEY";
        #endregion

        private async Task AnalyseImageLocalMachine(string localImage)
        {
            ComputerVisionClient client = Authenticate(_azureVisionEndpoint, _key);

            // the list of features we want to find in the image 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
                {
                    VisualFeatureTypes.Objects,
                    VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                    VisualFeatureTypes.Tags,
                    VisualFeatureTypes.Color, VisualFeatureTypes.Brands
                };

            Console.WriteLine($"Analysing local image {Path.GetFileName(localImage)}...");

            using (Stream imageStream = File.OpenRead(localImage))
            {
                try
                {
                    // Analyse the local image.
                    ImageAnalysis results = results = await client.AnalyzeImageInStreamAsync(imageStream, features);

                    // Summary of image content.
                    Console.WriteLine("Summary:");
                    foreach (var caption in results.Description.Captions)
                    {
                        Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
                    }
                    Console.WriteLine();
                    // Display categories the image is divided into.
                    Console.WriteLine("Categories:");
                    foreach (var category in results.Categories)
                    {
                        Console.WriteLine($"{category.Name} with confidence {category.Score}");
                    }
                    Console.WriteLine();
                    // Image tags with confidence score.
                    Console.WriteLine("Tags:");
                    foreach (var tag in results.Tags)
                    {
                        Console.WriteLine($"{tag.Name} {tag.Confidence}");
                    }
                    Console.WriteLine();
                    // Objects.
                    Console.WriteLine("Objects:");
                    foreach (var obj in results.Objects)
                    {
                        Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence}");
                    }
                    Console.WriteLine();
                    // detect Well-known brands
                    Console.WriteLine("Brands:");
                    foreach (var brand in results.Brands)
                    {
                        Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence}");
                    }
                    Console.WriteLine();

                    // colours                 
                    Console.WriteLine("Is black and white?: " + results.Color.IsBWImg);
                    Console.WriteLine("Dominant colors: " + string.Join(",", results.Color.DominantColors));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private ComputerVisionInsight ConvertImageAnalysisToInsight(ImageAnalysis imageAnalysis)
        {
            ComputerVisionInsight visionInsight = new ComputerVisionInsight();

            ImageDescriptionDetails imageDescription = imageAnalysis.Description;

            foreach (DetectedObject obj in imageAnalysis.Objects)
            {
                visionInsight.DetectedObjects.Add(new Entity.Item
                {
                    Name = obj.ObjectProperty,
                    Confidence = obj.Confidence
                });
            }

            foreach (DetectedBrand brand in imageAnalysis.Brands)
            {
                visionInsight.Brands.Add(new Entity.Item
                {
                    Name = brand.Name,
                    Confidence = brand.Confidence
                });
            }

            foreach (ImageTag tag in imageAnalysis.Tags)
            {
                visionInsight.Tags.Add(new Entity.Item
                {
                    Name = tag.Name,
                    Confidence = tag.Confidence
                });
            }

            return visionInsight;
        }

        private async Task<ImageAnalysis> AnalyseImageFromURL(string url)
        {
            ComputerVisionClient client = Authenticate(_azureVisionEndpoint, _key);

            // the list of features we want to find in the image 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
                {
                    VisualFeatureTypes.Objects,
                    VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                    VisualFeatureTypes.Tags,
                    VisualFeatureTypes.Color, VisualFeatureTypes.Brands
                };

            Console.WriteLine($"Analysing image from URL" + url);
            Console.WriteLine();

            try
            {
                // Analyze the local image.
                ImageAnalysis results = await client.AnalyzeImageAsync(url, features);

                // Summary of the image content.
                Console.WriteLine("Summary:");
                foreach (var caption in results.Description.Captions)
                {
                    Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
                }
                Console.WriteLine();

                // Display categories the image is divided into.
                Console.WriteLine("Categories:");
                foreach (var category in results.Categories)
                {
                    Console.WriteLine($"{category.Name} with confidence {category.Score}");
                }
                Console.WriteLine();
                // Image tags along with confidence score.
                Console.WriteLine("Tags:");
                foreach (var tag in results.Tags)
                {
                    Console.WriteLine($"{tag.Name} {tag.Confidence}");
                }
                Console.WriteLine();
                // any objects that exisit 
                Console.WriteLine("Objects:");
                foreach (var obj in results.Objects)
                {
                    Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence}");
                }
                Console.WriteLine();
                // detect Well-known brands
                Console.WriteLine("Brands:");
                foreach (var brand in results.Brands)
                {
                    Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence}");
                }
                Console.WriteLine();

                // colours                 
                Console.WriteLine("Is black and white?: " + results.Color.IsBWImg);
                Console.WriteLine("Dominant colors: " + string.Join(",", results.Color.DominantColors));
                Console.WriteLine();

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            { Endpoint = endpoint, };
            return client;
        }

        public async Task<ComputerVisionInsight> GetImageInsightsAsync(string url)
        {
            ImageAnalysis imageAnalysis = await AnalyseImageFromURL(url);

            return ConvertImageAnalysisToInsight(imageAnalysis);
        }
    }

}