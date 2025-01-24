import { ImageSize } from "./image-size";
import { ImagePosition } from "./image-position";
import { MandelbrotSize } from "./mandelbrot-size";

export class MandelbrotZoomParameter {
  constructor(
    public MousePosition: ImagePosition,
    public ZoomIn: boolean,
    public ZoomCount: number,
    public ImageSize: ImageSize,
    public CurrentMandelbrotSize: MandelbrotSize,
    public MaxIterations: number) {
  }
}
