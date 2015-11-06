//----------------------------------------------------------------------------
// <copyright file="WordStatisticsFile.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     This module implements word statistics entry
// </summary>
//----------------------------------------------------------------------------

namespace Microsoft.Tts.Offline
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Schema;
    using Microsoft.Tts.Offline.Common;
    using Microsoft.Tts.Offline.Utility;

    /// <summary>
    /// Word statistic file, which is generated by CorpusAnalyzer.
    /// </summary>
    public class WordStatisticsFile : XmlDataFile
    {
        #region Fields

        // xml node names and name space
        private readonly string XmlNameSpace = @"http://schemas.microsoft.com/tts";
        private readonly string WordsNodeName = "words";
        private readonly string WordNodeName = "w";
        private readonly string FrequencyNodeName = "f";
        private readonly string FrequencyCoverageScaleNodeName = "fcs";
        private readonly string TextNodeName = "t";
        private readonly string TypeNodeName = "tp";
        private readonly string SampleNodeName = "s";

        // control character is not allow to be written to xml 
        private readonly string InvalidWordPattern = @"[\p{C}]";

        private static XmlSchema _schema;
        private Dictionary<string, WordInfo> _items = new Dictionary<string, WordInfo>();

        #endregion

        #region Constuctors

        /// <summary>
        /// Initializes a new instance of the <see cref="WordStatisticsFile"/> class.
        /// </summary>
        public WordStatisticsFile()
        {
            // disable the indent of output xml to minimize the file size
            SaveSettings.IndentChars = string.Empty;
        }

        #endregion

        #region Enums

        /// <summary>
        /// Word type or review status of the word.
        /// </summary>
        public enum WordType
        {
            /// <summary>
            /// Current word is not reviewed, others are reviewed.
            /// </summary>
            NR,

            /// <summary>
            /// Current word meets the review requirement.
            /// </summary>
            Y,

            /// <summary>
            /// Current word doesn't meet the review requirement.
            /// </summary>
            N,

            /// <summary>
            /// Current word is not a valid word.
            /// </summary>
            X,

            /// <summary>
            /// Any word.
            /// </summary>
            ALL,
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Schema of script.xml.
        /// </summary>
        public override XmlSchema Schema
        {
            get
            {
                if (_schema == null)
                {
                    _schema = XmlHelper.LoadSchemaFromResource("Microsoft.Tts.Offline.Schema.wordstatistics.xsd");
                }

                return _schema;
            }
        }

        /// <summary>
        /// Gets Word list with their information.
        /// </summary>
        public Dictionary<string, WordInfo> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Override the PerformanceSave function.
        /// </summary>
        /// <param name="writer">Xml writer.</param>
        /// <param name="contentController">To control the saving content.</param>
        protected override void PerformanceSave(XmlWriter writer, object contentController)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            ContentController controller = null;
            if (contentController != null)
            {
                controller = contentController as ContentController;
            }

            int currentId = 0;
            bool isContentStart = contentController == null;
            bool isContentEnd = false;
            writer.WriteStartElement(WordsNodeName, XmlNameSpace);
            foreach (KeyValuePair<string, WordInfo> wordInfo in _items)
            {
                ++currentId;

                // update the flag marking content start and end
                if (controller != null)
                {
                    isContentEnd = controller.CheckContentEnd(currentId,
                        wordInfo.Key, wordInfo.Value.FrequencyCoverageScale);
                    if (!isContentStart)
                    {
                        isContentStart = controller.CheckContentStart(currentId,
                            wordInfo.Key, wordInfo.Value.FrequencyCoverageScale);
                    }
                }

                if (isContentStart && !Regex.Match(wordInfo.Key, InvalidWordPattern).Success &&
                    (controller == null || controller.WordTypeMatches(wordInfo.Value.WordType)))
                {
                    writer.WriteStartElement(WordNodeName);
                    writer.WriteElementString(FrequencyCoverageScaleNodeName,
                        wordInfo.Value.FrequencyCoverageScale.ToString("N04", CultureInfo.InvariantCulture));
                    writer.WriteElementString(FrequencyNodeName,
                        wordInfo.Value.Frequency.ToString(CultureInfo.InvariantCulture));
                    writer.WriteElementString(TextNodeName, wordInfo.Key);
                    writer.WriteElementString(TypeNodeName, wordInfo.Value.WordType.ToString());
                    writer.WriteElementString(SampleNodeName, wordInfo.Value.Sample);
                    writer.WriteEndElement();
                }

                if (isContentEnd)
                {
                    break;
                }
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Override the perfomanceLoad function
        /// It throws ArgumentException, FormatException.
        /// </summary>
        /// <param name="reader">Stream reader of the input file.</param>
        /// <param name="contentController">To control the loading content.</param>
        protected override void PerformanceLoad(StreamReader reader, object contentController)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            ContentController controller = null;
            if (contentController != null)
            {
                controller = contentController as ContentController;
            }

            int currentId = 0;
            _items.Clear();
            bool isContentStart = (contentController == null) ? true : false;
            bool isContentEnd = false;
            using (XmlTextReader xmlReader = new XmlTextReader(reader))
            {
                while (xmlReader.Read() && !isContentEnd)
                {
                    if (xmlReader.NodeType == XmlNodeType.Element &&
                        xmlReader.Name == WordNodeName)
                    {
                        ++currentId;
                        KeyValuePair<string, WordInfo> wordInfo = PerformanceLoadWord(xmlReader);

                        // update the flag marking content start and end
                        if (controller != null)
                        {
                            isContentEnd = controller.CheckContentEnd(currentId,
                                wordInfo.Key, wordInfo.Value.FrequencyCoverageScale);
                            if (!isContentStart)
                            {
                                isContentStart = controller.CheckContentStart(currentId,
                                    wordInfo.Key, wordInfo.Value.FrequencyCoverageScale);
                            }
                        }

                        if (isContentStart)
                        {
                            if (!_items.ContainsKey(wordInfo.Key) && 
                                (controller == null || controller.WordTypeMatches(wordInfo.Value.WordType)))
                            {
                                _items.Add(wordInfo.Key, wordInfo.Value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Perfomance loading word information.
        /// </summary>
        /// <param name="xmlReader">Xml reader.</param>
        /// <returns>The word info.</returns>
        private KeyValuePair<string, WordInfo> PerformanceLoadWord(XmlTextReader xmlReader)
        {
            string wordText = string.Empty;
            WordInfo wordInfo = new WordInfo();

            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals(WordNodeName))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals(FrequencyCoverageScaleNodeName))
                    {
                        xmlReader.Read();
                        wordInfo.FrequencyCoverageScale = double.Parse(xmlReader.Value, CultureInfo.InvariantCulture);
                    }
                    else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals(FrequencyNodeName))
                    {
                        xmlReader.Read();
                        wordInfo.Frequency = int.Parse(xmlReader.Value, CultureInfo.InvariantCulture);
                    }
                    else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals(TextNodeName))
                    {
                        xmlReader.Read();
                        wordText = xmlReader.Value;
                    }
                    else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals(TypeNodeName))
                    {
                        xmlReader.Read();
                        wordInfo.WordType = (WordType)Enum.Parse(typeof(WordType), xmlReader.Value, true);
                    }
                    else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals(SampleNodeName))
                    {
                        xmlReader.Read();
                        if (xmlReader.NodeType == XmlNodeType.Text)
                        {
                            wordInfo.Sample = xmlReader.Value;
                        }
                    }

                    if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name.Equals(WordNodeName))
                    {
                        break;
                    }
                }
            }

            return new KeyValuePair<string, WordInfo>(wordText, wordInfo);
        }

        #endregion

        #region Sub-classes

        /// <summary>
        /// Word information.
        /// </summary>
        public class WordInfo
        {
            #region Fields

            private double _frequencyCoverageScale;
            private int _frequency;
            private WordType _wordType;
            private string _sample;

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets Frequency coverage scale.
            /// </summary>
            public double FrequencyCoverageScale
            {
                get
                {
                    return _frequencyCoverageScale;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("frequencyCoverageScale");
                    } 

                    _frequencyCoverageScale = value; 
                }
            }

            /// <summary>
            /// Gets or sets Frequency of the word in corpus.
            /// </summary>
            public int Frequency
            {
                get
                {
                    return _frequency;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("frequency");
                    }

                    _frequency = value;
                }
            }

            /// <summary>
            /// Gets or sets LE review status.
            /// </summary>
            public WordType WordType
            {
                get { return _wordType; }
                set { _wordType = value; }
            }

            /// <summary>
            /// Gets or sets Sample sentence of the word.
            /// </summary>
            public string Sample
            {
                get { return _sample; }
                set { _sample = value; }
            }

            #endregion
        }

        /// <summary>
        /// Content controller for loading and saving this file.
        /// </summary>
        public class ContentController
        {
            #region Fields

            private string _startWord;
            private string _endWord;
            private int _startId;
            private int _endId;
            private double _startFcs;
            private double _endFcs;
            private WordType _wordType;
            private ControllerType _controllType;

            #endregion

            #region Contructions

            /// <summary>
            /// Initializes a new instance of the <see cref="ContentController"/> class.
            /// </summary>
            /// <param name="startId">Start id of the word list.</param>
            /// <param name="endId">End id of the word list.</param>
            public ContentController(int startId, int endId)
                : this(startId, endId, WordType.ALL)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ContentController"/> class.
            /// </summary>
            /// <param name="startFcs">Frequency coverage scale of the start word.</param>
            /// <param name="endFcs">Frequency coverage scale of the end word.</param>
            public ContentController(double startFcs, double endFcs)
                : this(startFcs, endFcs, WordType.ALL)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ContentController"/> class.
            /// </summary>
            /// <param name="startWord">Start word of the word list.</param>
            /// <param name="endWord">End word of the word list.</param>
            public ContentController(string startWord, string endWord)
                : this(startWord, endWord, WordType.ALL)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ContentController"/> class.
            /// </summary>
            /// <param name="startId">Start word id of the word list.</param>
            /// <param name="endId">End word id of the word list.</param>
            /// <param name="wordType">LE review status for filter.</param>
            public ContentController(int startId, int endId, WordType wordType)
            {
                if (startId <= 0)
                {
                    throw new ArgumentException("startId should be larger than 0");
                }

                if (endId <= 0)
                {
                    throw new ArgumentException("endId should be larger than 0");
                }

                _startId = Math.Min(startId, endId);
                _endId = Math.Max(startId, endId);
                _wordType = wordType;
                _controllType = ControllerType.ID;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ContentController"/> class.
            /// </summary>
            /// <param name="startWord">Start word of the word list.</param>
            /// <param name="endWord">End word of the word list.</param>
            /// <param name="wordType">LE review status for filter.</param>
            public ContentController(string startWord, string endWord, WordType wordType)
            {
                if (string.IsNullOrEmpty(startWord))
                {
                    throw new ArgumentNullException("startWord");
                }

                if (string.IsNullOrEmpty(endWord))
                {
                    throw new ArgumentNullException("endWord");
                }

                _startWord = startWord;
                _endWord = endWord;
                _wordType = wordType;
                _controllType = ControllerType.Word;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ContentController"/> class.
            /// </summary>
            /// <param name="startFcs">Frequency coverage scale of the start word.</param>
            /// <param name="endFcs">Frequency coverage scale of the end word.</param>
            /// <param name="wordType">LE review status for filter.</param>
            public ContentController(double startFcs, double endFcs, WordType wordType)
            {
                if (startFcs < 0)
                {
                    throw new ArgumentException("startFcs should be larger than or equal to 0");
                }

                if (startFcs >= 1)
                {
                    throw new ArgumentException("startFcs should be less than 1");
                }
                
                if (endFcs <= 0)
                {
                    throw new ArgumentException("startFcs should be larger than 0");
                }

                if (endFcs > 1)
                {
                    throw new ArgumentException("endFcs should be less than or equal to 1");
                }

                _startFcs = Math.Min(startFcs, endFcs);
                _endFcs = Math.Max(startFcs, endFcs);
                _wordType = wordType;
                _controllType = ControllerType.Fcs;
            }

            #endregion

            #region Enums

            /// <summary>
            /// Controller type enumarion.
            /// </summary>
            internal enum ControllerType
            {
                /// <summary>
                /// Controll by word text.
                /// </summary>
                Word,

                /// <summary>
                /// Controll by word id.
                /// </summary>
                ID,

                /// <summary>
                /// Controll by word frequency coverage scale.
                /// </summary>
                Fcs,

                /// <summary>
                /// Controll only by word type.
                /// </summary>
                None,
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets Start word id of the word list.
            /// </summary>
            public int StartId
            {
                get { return _startId; }
                set { _startId = value; }
            }

            /// <summary>
            /// Gets or sets End word id of the word list.
            /// </summary>
            public int EndId
            {
                get { return _endId; }
                set { _endId = value; }
            }

            /// <summary>
            /// Gets or sets Start word of the word list.
            /// </summary>
            public string StartWord
            {
                get { return _startWord; }
                set { _startWord = value; }
            }

            /// <summary>
            /// Gets or sets End word of the word list.
            /// </summary>
            public string EndWord
            {
                get { return _endWord; }
                set { _endWord = value; }
            }

            /// <summary>
            /// Gets or sets Frequency coverage scale of the start word.
            /// </summary>
            public double StartFcs
            {
                get { return _startFcs; }
                set { _startFcs = value; }
            }

            /// <summary>
            /// Gets or sets Frequency coverage scale of the end word.
            /// </summary>
            public double EndFcs
            {
                get { return _endFcs; }
                set { _endFcs = value; }
            }

            /// <summary>
            /// Gets or sets LE review status for filter.
            /// </summary>
            public WordType WordStatusType
            {
                get { return _wordType; }
                set { _wordType = value; }
            }

            /// <summary>
            /// Gets or sets Controller type.
            /// </summary>
            internal ControllerType ControllType
            {
                get { return _controllType; }
                set { _controllType = value; }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Check if the word is in the start of content range.
            /// </summary>
            /// <param name="id">Id of the word.</param>
            /// <param name="word">Word text.</param>
            /// <param name="fcs">Frequency coverage scale.</param>
            /// <returns>True for started.</returns>
            public bool CheckContentStart(int id, string word, double fcs)
            {
                bool isStart = true;

                switch (_controllType)
                {
                    case ControllerType.Word:
                        isStart = _startWord.Equals(word);
                        break;

                    case ControllerType.ID:
                        isStart = _startId == id ? true : false;
                        break;

                    case ControllerType.Fcs:
                        isStart = _startFcs <= fcs ? true : false;
                        break;
                }

                return isStart;
            }

            /// <summary>
            /// Check if the word is in the end of content range.
            /// </summary>
            /// <param name="id">Id of the word.</param>
            /// <param name="word">Word text.</param>
            /// <param name="fcs">Frequency coverage scale.</param>
            /// <returns>True for end.</returns>
            public bool CheckContentEnd(int id, string word, double fcs)
            {
                bool isEnd = true;
                switch (_controllType)
                {
                    case ControllerType.Word:
                        isEnd = _endWord.Equals(word);
                        break;

                    case ControllerType.ID:
                        isEnd = _endId == id ? true : false;
                        break;

                    case ControllerType.Fcs:
                        isEnd = _endFcs <= fcs ? true : false;
                        break;
                }

                return isEnd;
            }

            /// <summary>
            /// Check if the word type matches this controller.
            /// </summary>
            /// <param name="wordType">Word type.</param>
            /// <returns>True if matches.</returns>
            public bool WordTypeMatches(WordType wordType)
            {
                bool match = true;
                if (_wordType != WordType.ALL && wordType != _wordType)
                {
                    match = false;
                }

                return match;
            }

            #endregion
        }

        #endregion
    }
}