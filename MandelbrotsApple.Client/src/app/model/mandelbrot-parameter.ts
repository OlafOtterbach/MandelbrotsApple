import { CanvasSize } from "./canvas-size";
import { MandelbrotSize } from "./mandelbrot-size";

export class MandelbrotParameter {
  constructor(
    public CanvasSize: CanvasSize,
    public CurrentMandelbrotSize: MandelbrotSize,
    public MaxIterations: number) {
  }
}
