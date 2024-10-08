﻿namespace DeviantArtFs.Pages

type Page<'a> = {
    has_more: bool option
    next_offset: int option
    error_code: int option
    results: 'a list option
}