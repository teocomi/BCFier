using IPA.Bcfier.Models.Bcf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IPA.Bcfier.Services
{
    public class BcfMergeService
    {
        public async Task<BcfFile?> MergeBcfFilesAsync(List<Stream> bcfFileStreams)
        {
            try
            {
                BcfFile newBcf = null;
                foreach (var stream in bcfFileStreams)
                {
                    var bcfResult = await new BcfImportService().ImportBcfFileAsync(stream, "issue.bcf");
                    if (newBcf == null)
                    {
                        newBcf = bcfResult;
                        continue;
                    }

                    foreach (var mergedIssue in bcfResult.Topics)
                    {
                        //it's a new issue
                        if (!newBcf.Topics.Any(x => x.Id == mergedIssue.Id))
                        {
                            newBcf.Topics.Add(mergedIssue);
                            // TODO VIEWPOINTS?
                        }
                        //it exists, let's loop comments and views
                        else
                        {
                            var issue = newBcf.Topics.First(x => x.Id == mergedIssue.Id);
                            var newComments = mergedIssue.Comments.Where(x => issue.Comments.All(y => y.Id != x.Id)).ToList();
                            if (newComments.Any())
                            {
                                foreach (var newComment in newComments)
                                {
                                    issue.Comments.Add(newComment);
                                }
                            }

                            //sort comments
                            issue.Comments = issue.Comments.OrderByDescending(x => x.CreationDate).ToList();

                            var newViews = mergedIssue.Viewpoints.Where(x => issue.Viewpoints.All(y => y.Id != x.Id)).ToList();
                            if (newViews.Any())
                            {
                                foreach (var newView in newViews)
                                {
                                    issue.Viewpoints.Add(newView);
                                    // TODO SNAPSHOTS?
                                }
                            }
                        }
                    }
                }

                return newBcf;
            }
            catch
            {
                return null;
            }
        }
    }
}
