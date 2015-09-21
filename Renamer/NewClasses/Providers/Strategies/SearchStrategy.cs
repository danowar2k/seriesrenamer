using Renamer.NewClasses.Collector;

namespace Renamer.NewClasses.Providers.Strategies {
	public interface SearchStrategy {
		ParsedSearch search(string showname, string searchString);
	}
}
