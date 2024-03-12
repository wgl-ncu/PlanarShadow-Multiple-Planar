# Planar Shadow

平面阴影，可以支持多个地面。（有没有用？并没什么用，多个地面还是用shadowmap更好，主要用于练手）

实时阴影：

![](https://typora-picture-back-up.oss-cn-hangzhou.aliyuncs.com/planarshadow1.png)

平面阴影：

![](https://typora-picture-back-up.oss-cn-hangzhou.aliyuncs.com/planarShadow2.png)

主要是利用模板测试实现在多个平面投影

步骤：

1. 每一个地面对应一个模板值（0，8）渲染地面的时候写入该模板值。本项目修改的是默认的Lit shader![](https://typora-picture-back-up.oss-cn-hangzhou.aliyuncs.com/lit1.png)
2. 在渲染平面阴影的时候根据之前写入的模板值确定在哪里写入阴影![](https://typora-picture-back-up.oss-cn-hangzhou.aliyuncs.com/ps.png)

**解决被遮挡的平面渲染阴影的问题：**

由于渲染平面阴影的时候不会考虑是否被其他平面遮挡，会出现以下情况：

![](https://typora-picture-back-up.oss-cn-hangzhou.aliyuncs.com/pp.png)

解决方法就是采样shadowmap中的对应值，判断该点是否处于阴影中，如果处于阴影中则不渲染阴影。

![](https://typora-picture-back-up.oss-cn-hangzhou.aliyuncs.com/fragps.png)

效果：

![](https://typora-picture-back-up.oss-cn-hangzhou.aliyuncs.com/ps2.png)

结果还是采样了贴图，损失该算法的快的优势orz，所以此项目并不实用
