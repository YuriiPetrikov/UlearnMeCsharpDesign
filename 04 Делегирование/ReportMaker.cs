// Вставьте сюда финальное содержимое файла ReportMaker.cs
using Delegates.Reports;
using NUnit.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
    public interface IReportMaker
    {
        string MakeCaption(string caption);
        string BeginList();
        string MakeItem(string valueType, string entry);
        string EndList();
    }

    public interface IMakeStatistics<T>
    {
        T MakeStatistics(IEnumerable<double> _data);
        string Caption { get; }
    }

    public class MeanAndStdMakeStatistic : IMakeStatistics<MeanAndStd>
    {
        public string Caption
        {
            get
            {
                return "Mean and Std";
            }
        }

        public MeanAndStd MakeStatistics(IEnumerable<double> _data)
        {
            var data = _data.ToList();
            var mean = data.Average();
            var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

            return new MeanAndStd
            {
                Mean = mean,
                Std = std
            };
        }
    }

    public class MedianMakeStatistic : IMakeStatistics<double>
    {
        public string Caption
        {
            get
            {
                return "Median";
            }
        }

        public double MakeStatistics(IEnumerable<double> _data)
        {
            var list = _data.OrderBy(z => z).ToList();
            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

            return list[list.Count / 2];
        }
    }

    public class HtmlReportMaker : IReportMaker
    {
        public string BeginList()
        {
            return "<ul>";
        }

        public string EndList()
        {
            return "</ul>";
        }

        public string MakeCaption(string caption)
        {
            return $"<h1>{caption}</h1>";
        }

        public string MakeItem(string valueType, string entry)
        {
            return $"<li><b>{valueType}</b>: {entry}";
        }
    }

    public class MarkdownReportMaker : IReportMaker
    {
        public string BeginList()
        {
            return "";
        }

        public string EndList()
        {
            return "";
        }

        public string MakeCaption(string caption)
        {
            return $"## {caption}\n\n";
        }

        public string MakeItem(string valueType, string entry)
        {
            return $" * **{valueType}**: {entry}\n\n";
        }
    }

    public class ReportMaker<T>
    {
        protected IReportMaker reportMaker;
        protected IMakeStatistics<T> makeStatistics;
        public ReportMaker(IReportMaker reportMaker, IMakeStatistics<T> makeStatistics)
        {
            this.reportMaker = reportMaker;
            this.makeStatistics = makeStatistics;
        }

        public string MakeReport(IEnumerable<Measurement> measurements)
		{
            var data = measurements.ToList();
            var result = new StringBuilder();
            result.Append(reportMaker.MakeCaption(makeStatistics.Caption));
            result.Append(reportMaker.BeginList());
			result.Append(reportMaker.MakeItem("Temperature", makeStatistics.
				MakeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(reportMaker.MakeItem("Humidity", makeStatistics.
				MakeStatistics(data.Select(z => z.Humidity)).ToString()));
            result.Append(reportMaker.EndList());
            return result.ToString();
		}
    }

    public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
		{
			return new ReportMaker<MeanAndStd>(new HtmlReportMaker(), new MeanAndStdMakeStatistic()).MakeReport(data);
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data)
		{
			return new ReportMaker<double>(new MarkdownReportMaker(), new MedianMakeStatistic()).MakeReport(data);
        }

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
			return new ReportMaker<MeanAndStd>(new MarkdownReportMaker(), 
				new MeanAndStdMakeStatistic()).MakeReport(measurements);
        }

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
			return new ReportMaker<double>(new HtmlReportMaker(), new MedianMakeStatistic()).MakeReport(measurements);
        }
	}
}
