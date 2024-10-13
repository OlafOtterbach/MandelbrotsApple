export class MandelbrotParameter {
  constructor(
    public width: number,
    public height: number,
    public xMin: number,
    public xMax: number,
    public yMin: number,
    public yMax: number,
    public maxIterations: number) {
  }
}
