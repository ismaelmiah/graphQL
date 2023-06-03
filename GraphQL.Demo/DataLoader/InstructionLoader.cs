public class InstructorLoader : BatchDataLoader<Guid, InstructorDto>
{
    private readonly InstructorRepository _instructorRepository;
    public InstructorLoader(IBatchScheduler batchScheduler, InstructorRepository instructorRepository,
                 DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        _instructorRepository = instructorRepository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, InstructorDto>> LoadBatchAsync(IReadOnlyList<Guid> instructorIds, CancellationToken cancellationToken)
    {
        var instructors = await _instructorRepository.GetByIds(instructorIds);

        return instructors.ToDictionary(x => x.Id);
    }
}