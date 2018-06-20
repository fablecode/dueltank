import {FileSizePipe} from "../../../../app/shared/pipes/filesize.pipe";

describe("Pipe: File Size", () => {
  let pipe: FileSizePipe;

  beforeEach(() => {
    pipe = new FileSizePipe();
  });

  it("providing a value returns value", () => {
    expect(pipe.transform(50)).toBe("50 Bytes");
  });

  it('providing a negative value returns fallback', () => {
    expect(pipe.transform(-45)).toBe("0 Bytes");
  });

});
