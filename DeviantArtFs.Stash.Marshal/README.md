# DeviantArtFs.Stash.Marshal

An F# library (.NET Standard 2.0) to interact with the [Sta.sh API.](https://www.deviantart.com/developers/http/v1/20160316)

This library sits on top of DeviantArtFs and provides a StashRoot object that can be used to process reponses from the Sta.sh delta endpoint.

The delta response contains a list of entries that need to be applied in order. The interface ISerializedStashDeltaEntry is used to represent these endpoints:

	namespace DeviantArtFs {
		public interface ISerializedStashDeltaEntry {
			long? Itemid { get; }
			long? Stackid { get; }
			string Metadata { get; }
			int? Position { get; }
		}
	}

(This interface is actually defined in StashDeltaEntry.fs in the DeviantArtFs project.)

ISerializedStashDeltaEntry is implemented both by DeviantArtFs.StashDeltaEntry, the type used for responses from the server,
and by SerializedDeltaEntry, the type that StashRoot uses for export.
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
		return stashRoot.Save();
	}

	void Deserialize(IEnumerable<ISerializedStashDeltaEntry> list) {
		stashRoot.Clear();
        foreach (var x in list) {
            stashRoot.Apply(x);
        }
	}

See the StashTree project in the DeviantArtFs source code repository for a more concrete example.
