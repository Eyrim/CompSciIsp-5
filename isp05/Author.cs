using System;
using System.Collections.Generic;

namespace isp05
{
    public class Author
    {
        /// <summary>
        /// The ID of the author
        /// </summary>
        public string au_id { get; }

        /// <summary>
        /// The last name of the author
        /// </summary>
        public string au_lname { get; }

        /// <summary>
        /// The first name of the author
        /// </summary>
        public string au_fname { get; }

        /// <summary>
        /// The phone number of the author
        /// </summary>
        public string phone { get; }

        /// <summary>
        /// The address of the author
        /// </summary>
        public string address { get; }

        /// <summary>
        /// The state the author operates in
        /// </summary>
        public string state { get; }

        /// <summary>
        /// The zip code of the author
        /// </summary>
        public string zip { get; }

        /// <summary>
        /// The author's contract status
        /// </summary>
        public string contract { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorData"></param>
        public Author(List<string> authorData)
        {
            au_id = authorData[0];
            au_lname = authorData[1];
            au_fname = authorData[2];
            phone = authorData[3];
            address = authorData[4];
            state = authorData[5];
            zip = authorData[6];
            contract = authorData[7];
        }
    }
}