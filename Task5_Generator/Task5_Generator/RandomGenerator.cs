// Created by Ivan Chernuha 24.11.2014.
// chernuhaiv@gmail.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace Task5_Generator
{
    /// 
    class RandomGenerator 
    {
        #region Class members

        private HttpClient httpClient;
        private HttpClientHandler handler;

        //Notify if server is not available for requst or any 
        //error happened when we tried to connect
        private bool isServerAvailable;
        //Notify that we are trying to execute request to server
        //(to avoid simultaneous requesting)
        private volatile bool isRequestRunning;

        //According to technical requirements, 
        //generator must get sequence with 
        //custom upper bound "max" value, so let's divide 
        //http address to be able concatenate it with custom max
        private static string httpAddressBeforeMax = 
            "http://www.random.org/integers/?num=10&min=0&max=";
        private static string httpAddressAfterMax = 
            "&col=1&base=10&format=plain&rnd=new";
        private string Max;
        //Queue of random numbers got from random.org
        private volatile Queue<int> randoms = new Queue<int>();
        private Random randomBCL = new Random();

        #endregion

        public RandomGenerator()
        {
            handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            httpClient = new HttpClient(handler);
            //suppose at start, that server available
            isServerAvailable = true;
            isRequestRunning = false;
        }

        /// <summary>
        /// Generator returns random integers using random.org
        /// if it possible, otherwise from BCL random.
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public  IEnumerable<int> NextInt(int N)
        {
            Max = N.ToString();
            for (int i = 0; i < N; ++i )
            {
                //try to return numbers got from random.org
                if (randoms.Count != 0)
                    yield return randoms.Dequeue();
                //queue is empty, then try async request
                //to avoid simulaneous requesting
                else if (isServerAvailable)
                {
                    if (!isRequestRunning){
                        Task.Factory.StartNew(() => DoRequst()).Wait();
                        //timeout for request
                        Task.Delay(3000).Wait();
                        if(randoms.Count != 0)
                            yield return randoms.Dequeue();
                        else yield return randomBCL.Next(0, N);
                    }
                    else yield return randomBCL.Next(0, N);
                }
                else yield return randomBCL.Next(0, N);
            }
            randoms.Clear();
        }

        /// Try to get random numbers from server.
        private async Task DoRequst()
        {
            try
            {
                isRequestRunning = true;
                string s = httpAddressBeforeMax + Max + httpAddressAfterMax;
                HttpResponseMessage response = await httpClient.GetAsync(
                    httpAddressBeforeMax + Max + httpAddressAfterMax);
                //check for errors
                response.EnsureSuccessStatusCode();
                string randomNumbersResponce = await response.Content.ReadAsStringAsync();
                parseResponce(randomNumbersResponce);
            }
            catch (HttpRequestException hre)
            {
                isServerAvailable = false;
            }
            isRequestRunning = false;
        }

        /// <summary>
        /// Convert and add integers from 
        /// param string into queue
        /// </summary>
        /// <param name="numbers">
        /// Represents string of separated integers
        /// </param>
        private void parseResponce(string numbers)
        {
            string num = "";
            for (var i = 0; i < numbers.Length; ++i)
            {
                while (numbers[i] != '\n' )
                    num += numbers[i++];
                randoms.Enqueue(Int32.Parse(num));
                num = "";
            }
        }
        
    }
}
