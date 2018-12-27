# DeviantArtFs.Stash.Marshal

An F# library (.NET Standard 2.0) to interact with the [Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)

This library sits on top of DeviantArtFs and provides a StashRoot object that can be used to process reponses from the Sta.sh delta endpoint.

The delta response contains a list of entries that need to be applied in order. The interface IDeltaEntry is used to represent these endpoints:

	namespace DeviantArtFs {
		public interface IDeltaEntry {
			long? Itemid { get; }
			long? Stackid { get; }
			string Metadata { get; }
			int? Position { get; }
		}
	}

(This interface is actually defined in Types.fs in the DeviantArtFs project.)

IDeltaEntry is implemented both by DeviantArtFs.Stash.DeltaResultEntry, the type used for responses from the server,
and by SerializedDeltaEntry, the type that StashRoot's nodes use for export.
You can also implement it yourself (e.g. on an object representing a database row.)

Example usage (C#):

	string cursor = null;
	var stashRoot = new StashRoot();

	void Refresh() {
		var delta = await Stash.Delta.GetAllAsync(token, new Stash.DeltaAllRequest { Cursor = StashCursor });
		cursor = delta.Cursor;

		Deserialize(delta.Entries);
	}

	List<SerializedDeltaEntry> Serialize() {
		return stashRoot.Nodes.Select(c => c.Save()).ToList();
	}

	void Deserialize(IEnumerable<IDeltaEntry> list) {
		stashRoot.Clear();
        foreach (var x in list) {
            stashRoot.Apply(x);
        }
	}
