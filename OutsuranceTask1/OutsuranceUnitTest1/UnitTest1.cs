using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutsuranceTask1;
using System.IO;

namespace OutsuranceUnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGoodFile()
        {
            string csvFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodCSV.csv");
            string namesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodNames.txt");
            string addressesFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "GoodAddresses.txt");
            CreateGoodCSV(csvFileName);
            ProcessFile processFile = new ProcessFile();
            var result = processFile.ValidateCSV(csvFileName);
            bool hasHeader =  processFile.ValidateCSVHeader(csvFileName) == "True";
            result = processFile.ProcessCSV(csvFileName, hasHeader);
            result = processFile.ExportNames(namesFileName);
            result = processFile.ExportAddresses(addressesFileName);
            Assert.IsTrue((string.IsNullOrEmpty(result)) & (File.Exists(namesFileName)) & (File.Exists(addressesFileName)));
            DeleteFile(csvFileName);
            DeleteFile(namesFileName);
            DeleteFile(addressesFileName);
        }

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
    }
}
