export class MandelbrotParameter {
  constructor(
    public Width: number,
    public Height: number,
    public XMin: number,
    public XMax: number,
    public YMin: number,
    public YMax: number,
    public MaxIterations: number) {
  }
}
