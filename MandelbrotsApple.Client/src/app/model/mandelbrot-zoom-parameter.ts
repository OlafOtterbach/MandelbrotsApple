import { CanvasSize } from "./canvas-size";
import { CanvasPosition } from "./canvas-position";
import { MandelbrotSize } from "./mandelbrot-size";

export class MandelbrotZoomParameter {
  constructor(
    public MousePosition: CanvasPosition,
    public ZoomIn: boolean,
    public CanvasSize: CanvasSize,
    public CurrentMandelbrotSize: MandelbrotSize,
    public MaxIterations: number) {
  }
}
