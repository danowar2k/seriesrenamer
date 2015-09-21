namespace Renamer.NewClasses.Config {
	/// <summary>
	/// Values that the configuration file parser and writer
	/// use for determing the state of parsing
	/// </summary>
	enum ParserMode : int {
		/// <summary>
		/// An error happened when parsing
		/// </summary>
		Error,
		/// <summary>
		/// When parsing a multi value property
		/// </summary>
		MultiValueProperty,
		/// <summary>
		/// Otherwise, normal behaviour
		/// </summary>
		Normal
	}
}
