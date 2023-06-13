# s3-md5-etag-algorithm
An algorithm to calculate s3 object md5 hash in C#

Every S3 object has an associated Entity tag or ETag which can be used for file and object comparison.
We’ll cover the advantages of using the provided AWS ETag for comparison, as well as how to calculate the ETag of a local file.


Sample usage :-
```
dotnet CalculateS3EtagAlgo.dll -f "<file_path>" CalculateS3EtagAlgo.dll -c <chunk_size_in_mb>
```
