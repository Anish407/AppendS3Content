// See https://aka.ms/new-console-template for more information
using Amazon;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Text;
Console.WriteLine("Enter New Mesage");
string newMessage = Console.ReadLine();
await ReadS3TextFile("demobucket555", "test.txt");

async Task<string> ReadS3TextFile(string bucketName, string keyName)
{
    try
    {
        StringBuilder sb = new StringBuilder();
        await ReadContentFromS3(bucketName, keyName, sb);

        AddNewContent(newMessage, sb);

        await WriteToS3(bucketName, keyName, sb);

        return sb.ToString();
    }
    catch (Exception ex)
    {
        throw;
    }
}

async Task<StringBuilder> ReadContentFromS3(string bucketName, string keyName, StringBuilder appendContent)
{
    try
    {
        using (var client = new AmazonS3Client(RegionEndpoint.EUNorth1))
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            using (var response = await client.GetObjectAsync(request))
            using (var streamReader = new StreamReader(response.ResponseStream))
            {
                appendContent.Append(await streamReader.ReadToEndAsync());
            }

            return appendContent;
        }
    }
    catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        // create new File
        throw;
    }
    catch (Exception ex)
    {
        throw;
    }
}
Console.WriteLine("Hello, World!");

static void AddNewContent(string newMessage, StringBuilder sb)
{
    sb.Append(Environment.NewLine);
    sb.Append(newMessage);
}

static async Task WriteToS3(string bucketName, string keyName, StringBuilder sb)
{
    try
    {
        using (var client = new AmazonS3Client(RegionEndpoint.EUNorth1))
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,

            };

            await client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,
                ContentBody = sb.ToString()
            });
        }
    }
    catch (AmazonS3Exception ex)
    {
        // create new File
        throw;
    }
    catch (Exception ex)
    {
        throw;
    }
}