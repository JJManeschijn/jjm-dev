using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutsuranceTask1;
using System.IO;
using System.Collections.Generic;

namespace OutsuranceUnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Test process where input data is good. 
        /// Expected results to pass:
        /// 1. Process returnd string must be empty.
        /// 2. Output Names file must exist.
        /// 3. Output Addresses file must exist.
        /// </summary>
        [TestMethod]
        public void TestGoodFile()
        {
            string csvFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodCSV.csv");
            string namesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodNames.txt");
            string addressesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodAddresses.txt");
            CreateGoodCSV(csvFileName);
            ProcessFile processFile = new ProcessFile();
            var result = processFile.ValidateCSV(csvFileName);
            bool hasHeader = processFile.ValidateCSVHeader(csvFileName) == "True";
            result = processFile.ProcessCSV(csvFileName, hasHeader);
            result = processFile.ExportNames(namesFileName);
            result = processFile.ExportAddresses(addressesFileName);
            Assert.IsTrue((string.IsNullOrEmpty(result)) & (File.Exists(namesFileName)) & (File.Exists(addressesFileName)));
            DeleteFile(csvFileName);
            DeleteFile(namesFileName);
            DeleteFile(addressesFileName);
        }

        /// <summary>
        /// Test process where input data is NOT good. 
        /// Expected results to pass:
        /// 1. Process return string must NOT be empty (contains error information).
        /// 2. Output Names file must NOT exist.
        /// 3. Output Addresses file must NOT exist.
        /// </summary>
        [TestMethod]
        public void TestBadFile()
        {
            string csvFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BadCSV.csv");
            string namesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BadNames.txt");
            string addressesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BadAddresses.txt");
            CreateBadCSV(csvFileName);
            ProcessFile processFile = new ProcessFile();
            var result = processFile.ValidateCSV(csvFileName);
            bool hasHeader = processFile.ValidateCSVHeader(csvFileName) == "True";
            result = processFile.ProcessCSV(csvFileName, hasHeader);
            result = processFile.ExportNames(namesFileName);
            result = processFile.ExportAddresses(addressesFileName);
            Assert.IsTrue((!string.IsNullOrEmpty(result)) | (!File.Exists(namesFileName)) | (!File.Exists(addressesFileName)));
            DeleteFile(csvFileName);
            DeleteFile(namesFileName);
            DeleteFile(addressesFileName);
        }

        /// <summary>
        /// Test Names output grouping and sorting results by perform file comparison between hard-coded file containing expected output and physical output file. 
        /// Expected results to pass:
        /// 1. Process return string must be empty.
        /// 2. Generated Hard-coded Names file must exist.
        /// 3. Output Names file must exist.
        /// 4. File comparison must return TRUE.
        /// </summary>
        [TestMethod]
        public void TestNamesOutput()
        {
            string csvFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodCSV.csv");
            string namesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodNames.txt");
            string nameTestFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "NamesTest.txt");
            CreateGoodCSV(csvFileName);
            CreateNamesTestFile(nameTestFile);
            ProcessFile processFile = new ProcessFile();
            var result = processFile.ValidateCSV(csvFileName);
            bool hasHeader = processFile.ValidateCSVHeader(csvFileName) == "True";
            result = processFile.ProcessCSV(csvFileName, hasHeader);
            result = processFile.ExportNames(namesFileName);
            Assert.IsTrue((string.IsNullOrEmpty(result)) & (File.Exists(namesFileName)) & (File.Exists(nameTestFile)) & (CompareFileContent(namesFileName, nameTestFile)));
            DeleteFile(csvFileName);
            DeleteFile(namesFileName);
            DeleteFile(nameTestFile);
        }

        /// <summary>
        /// Test Addresses output grouping and sorting results by perform file comparison between hard-coded file containing expected output and physical output file. 
        /// Expected results to pass:
        /// 1. Process return string must be empty.
        /// 2. Generated Hard-coded Addresses file must exist.
        /// 3. Output Addresses file must exist.
        /// 4. File comparison must return TRUE.
        /// </summary>
        [TestMethod]
        public void TestAddressesOutput()
        {
            string csvFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodCSV.csv");
            string addressesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodAddresses.txt");
            string addressTestFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "AddressesTest.txt");
            CreateGoodCSV(csvFileName);
            CreateAddressesTestFile(addressTestFile);
            ProcessFile processFile = new ProcessFile();
            var result = processFile.ValidateCSV(csvFileName);
            bool hasHeader = processFile.ValidateCSVHeader(csvFileName) == "True";
            result = processFile.ProcessCSV(csvFileName, hasHeader);
            result = processFile.ExportAddresses(addressesFileName);
            Assert.IsTrue((string.IsNullOrEmpty(result)) & (File.Exists(addressesFileName)) & (File.Exists(addressTestFile)) & (CompareFileContent(addressesFileName, addressTestFile)));
            DeleteFile(csvFileName);
            DeleteFile(addressesFileName);
            DeleteFile(addressTestFile);
        }

        /// <summary>
        /// Test Names output grouping and sorting results by perform comparison between output file contents loaded to list and list of expected results. 
        /// Expected results to pass:
        /// 1. Process return string must be empty.
        /// 2. Generated Hard-coded Addresses file must exist.
        /// 3. Output Addresses file must exist.
        /// 4. File comparison must return TRUE.
        /// </summary>
        [TestMethod]
        public void TestNamesOutputValidation()
        {
            string csvFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodCSV.csv");
            string namesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodNames.txt");
            CreateGoodCSV(csvFileName);
            ProcessFile processFile = new ProcessFile();
            var result = processFile.ValidateCSV(csvFileName);
            bool hasHeader = processFile.ValidateCSVHeader(csvFileName) == "True";
            result = processFile.ProcessCSV(csvFileName, hasHeader);
            result = processFile.ExportNames(namesFileName);
            Assert.IsTrue((string.IsNullOrEmpty(result)) & (File.Exists(namesFileName)) & (ValidateOutput(namesFileName, BuildNamesValidationSet())));
            DeleteFile(csvFileName);
            DeleteFile(namesFileName);
        }

        /// <summary>
        /// Test Addresses output grouping and sorting results by perform comparison between output file contents loaded to list and list of expected results. 
        /// Expected results to pass:
        /// 1. Process return string must be empty.
        /// 2. Generated Hard-coded Addresses file must exist.
        /// 3. Output Addresses file must exist.
        /// 4. File comparison must return TRUE.
        /// </summary>
        [TestMethod]
        public void TestAddressesOutputValidation()
        {
            string csvFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodCSV.csv");
            string addressesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodAddresses.txt");
            CreateGoodCSV(csvFileName);
            ProcessFile processFile = new ProcessFile();
            var result = processFile.ValidateCSV(csvFileName);
            bool hasHeader = processFile.ValidateCSVHeader(csvFileName) == "True";
            result = processFile.ProcessCSV(csvFileName, hasHeader);
            result = processFile.ExportAddresses(addressesFileName);
            Assert.IsTrue((string.IsNullOrEmpty(result)) & (File.Exists(addressesFileName)) & (ValidateOutput(addressesFileName, BuildAddressValidationSet())));
            DeleteFile(csvFileName);
            DeleteFile(addressesFileName);
        }

        void CreateGoodCSV(string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.WriteLine("Name,Address");
                streamWriter.WriteLine("Johnson Smith,16 Clifton Rd");
                streamWriter.WriteLine("Tim Johnson,22 Jones Rd");
                streamWriter.WriteLine("Heinrich Jones,31 Clifton Rd");
                streamWriter.WriteLine("Gareth Jones,147 17th Ave");
                streamWriter.WriteLine("Koos van der Merwe,7de Laan 22");
                streamWriter.WriteLine("Matt Brown,12 Acton St");
                streamWriter.WriteLine("Peter Brown,9a Weaver Str");
                streamWriter.WriteLine("Sally Smith,31b Church Str");
                streamWriter.WriteLine("Joey Gordon,11a 11th Ave");
                streamWriter.WriteLine("John Smith,256 West Str");
            }
        }

        void CreateNamesTestFile(string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.WriteLine("Smith,3");
                streamWriter.WriteLine("Brown,2");
                streamWriter.WriteLine("Johnson,2");
                streamWriter.WriteLine("Jones,2");
                streamWriter.WriteLine("der,1");
                streamWriter.WriteLine("Gareth,1");
                streamWriter.WriteLine("Gordon,1");
                streamWriter.WriteLine("Heinrich,1");
                streamWriter.WriteLine("Joey,1");
                streamWriter.WriteLine("John,1");
                streamWriter.WriteLine("Koos,1");
                streamWriter.WriteLine("Matt,1");
                streamWriter.WriteLine("Merwe,1");
                streamWriter.WriteLine("Peter,1");
                streamWriter.WriteLine("Sally,1");
                streamWriter.WriteLine("Tim,1");
                streamWriter.WriteLine("van,1");
            }
        }

        void CreateAddressesTestFile(string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.WriteLine("11a 11th Ave");
                streamWriter.WriteLine("147 17th Ave");
                streamWriter.WriteLine("22 7de Laan");
                streamWriter.WriteLine("12 Acton St");
                streamWriter.WriteLine("31b Church Str");
                streamWriter.WriteLine("16 Clifton Rd");
                streamWriter.WriteLine("31 Clifton Rd");
                streamWriter.WriteLine("22 Jones Rd");
                streamWriter.WriteLine("9a Weaver Str");
                streamWriter.WriteLine("256 West Str");
            }
        }
        void CreateBadCSV(string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.WriteLine("Name,Address,");
                streamWriter.WriteLine("Johnson,Smith,16 Clifton Rd");
                streamWriter.WriteLine("");
                streamWriter.WriteLine(",Heinrich Jones,31 Clifton Rd");
            }

        }

        /// <summary>
        /// Performs a comparison to check if two files have exactly the same contents. 
        /// In this case the comparison is used to check the hard-coded expected results with the actual process output files for 
        /// correct number of lines as well as sorting and grouping.
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        bool CompareFileContent(string file1, string file2)
        {
            bool result = true;
            int file1byte;
            int file2byte;
            FileStream fileStream1;
            FileStream fileStream2;

            fileStream1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
            fileStream2 = new FileStream(file2, FileMode.Open, FileAccess.Read);

            // Check if file sizes are the same
            if (fileStream1.Length != fileStream2.Length)
            {
                fileStream1.Close();
                fileStream2.Close();
                result = false;
            }

            if (result)
            {
                // do byte comparison
                do
                {
                    // Read one byte from each file.
                    file1byte = fileStream1.ReadByte();
                    file2byte = fileStream2.ReadByte();
                }
                while ((file1byte == file2byte) && (file1byte != -1));

                fileStream1.Close();
                fileStream2.Close();

                result = ((file1byte - file2byte) == 0);
            }
            return result;
        }

        void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// Another method to test processed results vs expected results for processing and sorting & grouping.
        /// </summary>
        /// <param name="outputFile"></param>
        /// <param name="validationDataSet"></param>
        /// <returns></returns>
        bool ValidateOutput(string outputFile, List<string> validationDataSet)
        {
            bool result = true;

            //load results from output file to list
            var outputDataSet = new List<string>();
            using (StreamReader streamReader = new StreamReader(outputFile))
            {
                while (!streamReader.EndOfStream)
                {
                    outputDataSet.Add(streamReader.ReadLine());
                }
            }
            //compare output dataset with validation dataset
            if (outputDataSet.Count != validationDataSet.Count)
            {
                result = false;
            }
            else
            {
                for (int i = 0; i < outputDataSet.Count; i++)
                {
                    if (outputDataSet[i] != validationDataSet[i])
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        List<string> BuildNamesValidationSet()
        {
            var result = new List<string>
            {
                "Smith,3",
                "Brown,2",
                "Johnson,2",
                "Jones,2",
                "der,1",
                "Gareth,1",
                "Gordon,1",
                "Heinrich,1",
                "Joey,1",
                "John,1",
                "Koos,1",
                "Matt,1",
                "Merwe,1",
                "Peter,1",
                "Sally,1",
                "Tim,1",
                "van,1",
            };
            return result;
        }

        List<string> BuildAddressValidationSet()
        {
            var result = new List<string>
            {
                "11a 11th Ave",
                "147 17th Ave",
                "22 7de Laan",
                "12 Acton St",
                "31b Church Str",
                "16 Clifton Rd",
                "31 Clifton Rd",
                "22 Jones Rd",
                "9a Weaver Str",
                "256 West Str",
            };
            return result;
        }
    }
}



