# DeviantArtFs.Stash.Marshal

An F# library (.NET Standard 2.0) to interact with the [Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)

This library sits on top of DeviantArtFs and provides a StashRoot object that can be used to process reponses from the Sta.sh delta endpoint.

Example usage (C#):

	string cursor = null;
	var stashRoot = new StashRoot();

	void Refresh() {
		var delta = await Stash.Delta.GetAllAsync(token, new Stash.DeltaAllRequest { Cursor = StashCursor });
		cursor = delta.Cursor;

		foreach (DeltaResultEntry entry in delta.Entries) {
			stashRoot.Apply(new WrappedDeltaEntry(entry));
		}
	}